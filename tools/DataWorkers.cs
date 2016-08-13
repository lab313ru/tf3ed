using Helpers;
using System;
using System.IO;

namespace tf3ed.tools
{
    internal static class RleCompressor
    {
        private const byte NIBBLES_IN_LONG = 8;
        private const byte NIBBLES_IN_BYTE = 2;
        private const byte BITS_IN_NIBBLE = 4;

        /// <summary>
        /// Decompresses RLE packed data
        /// </summary>
        /// <param name="data">Compressed byte array</param>
        /// <param name="offset">Initial offset for compressed data</param>
        /// <returns></returns>
        public static byte[] DecompressData(byte[] data, int offset = 0)
        {
            int read_off = offset;
            int write_off = 0;

            int out_size = (int)data.ReadLong(read_off);
            byte[] output = new byte[out_size];

            int longs_to_write = out_size / 4;
            read_off += 4;

            uint long_to_write = 0;
            byte nibbles_in_long = NIBBLES_IN_LONG;

            while (longs_to_write > 0)
            {
                byte b = data[read_off++];

                byte nib_count = (byte)(b & 0xF);
                byte nibble = (byte)(b >> 4);

                int nibbles_to_write = 0;

                if (nib_count < 0xD)
                {
                    nibbles_to_write = nib_count;
                }
                else if (nib_count == 0xD)
                {
                    nibbles_to_write = data[read_off++];
                }
                else
                {
                    nibbles_to_write = data.ReadWord(read_off);
                    read_off += 2;

                    if (nib_count != 0xE)
                    {
                        nibbles_to_write += 0x10000;
                    }
                }

                for (int i = 0; i < nibbles_to_write + 1; ++i)
                {
                    long_to_write = (long_to_write << 4) | nibble;

                    if (--nibbles_in_long == 0)
                    {
                        output.WriteLong(write_off, long_to_write);

                        write_off += 4;
                        longs_to_write--;

                        nibbles_in_long = NIBBLES_IN_LONG;

                        if (longs_to_write == 0)
                        {
                            break;
                        }
                    }
                }
            }

            return output;
        }

        public static byte[] CompressData(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            output.WriteLong((uint)data.Length);

            int read_off = 0;

            int longs_size = data.Length / 4;
            byte[] nibbles = new byte[longs_size * NIBBLES_IN_LONG];

            while (read_off < data.Length)
            {
                uint l = data.ReadLong(read_off);

                for (int j = 0; j < NIBBLES_IN_LONG; ++j)
                {
                    nibbles[read_off * NIBBLES_IN_BYTE + j] = (byte)((l >> ((NIBBLES_IN_LONG - j - 1) * BITS_IN_NIBBLE)) & 0xF);
                }

                read_off += NIBBLES_IN_LONG / NIBBLES_IN_BYTE;
            }

            int i = 0;
            int nib_count = 1;
            for (i = 0; i < nibbles.Length;)
            {
                byte nibble = nibbles[i];
                nib_count = 1;

                while (i + nib_count < nibbles.Length && nibble == nibbles[i + nib_count])
                {
                    nib_count++;
                }

                byte token = (byte)(nibble << 4);
                nib_count--;

                if (nib_count < 0xD)
                {
                    token |= (byte)(nib_count & 0xF);
                    output.WriteByte(token);
                }
                else if (nib_count >= 0xD && nib_count <= 0xFF)
                {
                    token |= 0xD;
                    output.WriteByte((byte)nib_count);
                }
                else if (nib_count >= 0x100 && nib_count <= 0xFFFF)
                {
                    token |= 0xE;
                    output.WriteWord((ushort)nib_count);
                }
                else
                {
                    token |= 0xF;
                    output.WriteWord((ushort)(nib_count - 0x10000));
                }

                i += (nib_count + 1);
            }

            if ((output.Length & 1) != 0)
            {
                output.WriteByte(0xFF);
            }

            return output.ToArray();
        }
    }

    internal static class LzhCompressor
    {
        private static BinaryReader infile = null;
        private static BinaryWriter outfile = null;
        private static int textsize = 0, codesize = 0;

        /********** LZSS compression **********/
        private const int N = 4096;	/* buffer size */
        private const int F = 60;	/* lookahead buffer size */
        private const int THRESHOLD = 2;
        private const int NIL = N;	/* leaf of tree */

        private static byte[] text_buf = new byte[N + F - 1];
        private static int[] lson = new int[N + 1];
        private static int[] rson = new int[N + 257];
        private static int[] dad = new int[N + 1];
        private static int match_position, match_length;

        private static void Clear()
        {
            for (int i = 0; i < freq.Length; i++) freq[i] = 0;
            for (int i = 0; i < prnt.Length; i++) prnt[i] = 0;
            for (int i = 0; i < text_buf.Length; i++) text_buf[i] = 0;
            for (int i = 0; i < lson.Length; i++) lson[i] = 0;
            for (int i = 0; i < dad.Length; i++) dad[i] = 0;
            for (int i = 0; i < rson.Length; i++) rson[i] = 0;

            textsize = codesize = match_length = match_position = 0;
            getbuf = putbuf = 0;
            getlen = putlen = 0;
        }

        private static void InitTree()  /* initialize trees */
        {
            int i;

            for (i = N + 1; i <= N + 256; i++)
                rson[i] = NIL;			/* root */
            for (i = 0; i < N; i++)
                dad[i] = NIL;			/* node */
        }

        private static unsafe void InsertNode(int r)  /* insert to tree */
        {
            int i, p;
            uint c;

            fixed (byte* key = &text_buf[r])
            {
                p = N + 1 + key[0];
                rson[r] = lson[r] = NIL;
                match_length = 0;
                for (;;)
                {
                    if (rson[p] != NIL)
                        p = rson[p];
                    else
                    {
                        rson[p] = r;
                        dad[r] = p;
                        return;
                    }

                    for (i = 1; i < F; i++)
                        if ((key[i] - text_buf[p + i]) != 0)
                            break;
                    if (i > THRESHOLD)
                    {
                        if (i > match_length)
                        {
                            match_position = ((r - p) & (N - 1)) - 1;
                            if ((match_length = i) >= F)
                                break;
                        }
                        if (i == match_length)
                        {
                            c = (uint)((r - p) & (N - 1)) - 1;
                            if (c < (uint)match_position)
                            {
                                match_position = (int)c;
                            }
                        }
                    }
                }
                dad[r] = dad[p];
                lson[r] = lson[p];
                rson[r] = rson[p];
                dad[lson[p]] = r;
                dad[rson[p]] = r;
                if (rson[dad[p]] == p)
                    rson[dad[p]] = r;
                else
                    lson[dad[p]] = r;
                dad[p] = NIL; /* remove p */
            }
        }

        private static void DeleteNode(int p)  /* remove from tree */
        {
            int q;

            if (dad[p] == NIL)
                return;         /* not registered */
            if (rson[p] == NIL)
                q = lson[p];
            else
            if (lson[p] == NIL)
                q = rson[p];
            else
            {
                q = lson[p];
                if (rson[q] != NIL)
                {
                    do
                    {
                        q = rson[q];
                    } while (rson[q] != NIL);
                    rson[dad[q]] = lson[q];
                    dad[lson[q]] = dad[q];
                    lson[q] = lson[p];
                    dad[lson[p]] = q;
                }
                rson[q] = rson[p];
                dad[rson[p]] = q;
            }
            dad[q] = dad[p];
            if (rson[dad[p]] == p)
                rson[dad[p]] = q;
            else
                lson[dad[p]] = q;
            dad[p] = NIL;
        }

        /* Huffman coding */

        private const int N_CHAR = (256 - THRESHOLD + F);
        /* kinds of characters (character code = 0..N_CHAR-1) */
        private const int T = (N_CHAR * 2 - 1);	/* size of table */
        private const int R = (T - 1);			/* position of root */
        private const int MAX_FREQ = 0x8000;		/* updates tree when the root frequency comes to this value. */

        /* table for encoding and decoding the upper 6 bits of position */

        /* for encoding */

        private static readonly byte[] p_len = new byte[64] {
            0x03, 0x04, 0x04, 0x04, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x06, 0x06, 0x06, 0x06,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08,
            0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08
        };

        private static readonly byte[] p_code = new byte[64] {
            0x00, 0x20, 0x30, 0x40, 0x50, 0x58, 0x60, 0x68,
            0x70, 0x78, 0x80, 0x88, 0x90, 0x94, 0x98, 0x9C,
            0xA0, 0xA4, 0xA8, 0xAC, 0xB0, 0xB4, 0xB8, 0xBC,
            0xC0, 0xC2, 0xC4, 0xC6, 0xC8, 0xCA, 0xCC, 0xCE,
            0xD0, 0xD2, 0xD4, 0xD6, 0xD8, 0xDA, 0xDC, 0xDE,
            0xE0, 0xE2, 0xE4, 0xE6, 0xE8, 0xEA, 0xEC, 0xEE,
            0xF0, 0xF1, 0xF2, 0xF3, 0xF4, 0xF5, 0xF6, 0xF7,
            0xF8, 0xF9, 0xFA, 0xFB, 0xFC, 0xFD, 0xFE, 0xFF
        };

        /* for decoding */

        private static readonly byte[] d_code = new byte[256] {
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01,
            0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01, 0x01,
            0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02,
            0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02, 0x02,
            0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
            0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
            0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08,
            0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09, 0x09,
            0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A, 0x0A,
            0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B, 0x0B,
            0x0C, 0x0C, 0x0C, 0x0C, 0x0D, 0x0D, 0x0D, 0x0D,
            0x0E, 0x0E, 0x0E, 0x0E, 0x0F, 0x0F, 0x0F, 0x0F,
            0x10, 0x10, 0x10, 0x10, 0x11, 0x11, 0x11, 0x11,
            0x12, 0x12, 0x12, 0x12, 0x13, 0x13, 0x13, 0x13,
            0x14, 0x14, 0x14, 0x14, 0x15, 0x15, 0x15, 0x15,
            0x16, 0x16, 0x16, 0x16, 0x17, 0x17, 0x17, 0x17,
            0x18, 0x18, 0x19, 0x19, 0x1A, 0x1A, 0x1B, 0x1B,
            0x1C, 0x1C, 0x1D, 0x1D, 0x1E, 0x1E, 0x1F, 0x1F,
            0x20, 0x20, 0x21, 0x21, 0x22, 0x22, 0x23, 0x23,
            0x24, 0x24, 0x25, 0x25, 0x26, 0x26, 0x27, 0x27,
            0x28, 0x28, 0x29, 0x29, 0x2A, 0x2A, 0x2B, 0x2B,
            0x2C, 0x2C, 0x2D, 0x2D, 0x2E, 0x2E, 0x2F, 0x2F,
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
            0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
        };

        private static readonly byte[] d_len = new byte[256] {
            0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
            0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
            0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
            0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03, 0x03,
            0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
            0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
            0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
            0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
            0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
            0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04, 0x04,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05, 0x05,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06, 0x06,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07, 0x07,
            0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08,
            0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08, 0x08,
        };

        private static uint[] freq = new uint[T + 1];	/* frequency table */
        private static int[] prnt = new int[T + N_CHAR];	/* pointers to parent nodes, except for the elements [T..T + N_CHAR - 1] which are used to get
                                                     * the positions of leaves corresponding to the codes. */
        private static int[] son = new int[T];		/* pointers to child nodes (son[], son[] + 1) */
        private static uint getbuf = 0;
        private static byte getlen = 0;

        private static int GetBit()	/* get one bit */
        {
            uint i;

            while (getlen <= 8)
            {
                if (infile.BaseStream.Position < infile.BaseStream.Length)
                    i = infile.ReadByte();
                else
                    i = 0;

                getbuf |= i << (8 - getlen);
                getlen += 8;
            }
            i = getbuf;
            getbuf <<= 1;
            getlen--;
            return (int)((i & 0x8000) >> 15);
        }

        private static int GetByte()	/* get one byte */
        {
            uint i;

            while (getlen <= 8)
            {
                if (infile.BaseStream.Position < infile.BaseStream.Length)
                    i = infile.ReadByte();
                else
                    i = 0;

                getbuf |= i << (8 - getlen);
                getlen += 8;
            }
            i = getbuf;
            getbuf <<= 8;
            getlen -= 8;
            return (int)((i & 0xff00) >> 8);
        }

        private static uint putbuf = 0;
        private static byte putlen = 0;

        private static void Putcode(int l, uint c)		/* output c bits of code */
        {
            putbuf |= c >> putlen;
            if ((putlen += (byte)l) >= 8)
            {
                outfile.Write((byte)(putbuf >> 8));
                if ((putlen -= 8) >= 8)
                {
                    outfile.Write((byte)putbuf);
                    codesize += 2;
                    putlen -= 8;
                    putbuf = c << (l - putlen);
                }
                else
                {
                    putbuf <<= 8;
                    codesize++;
                }
            }
        }

        /* initialization of tree */

        private static void StartHuff()
        {
            int i, j;

            for (i = 0; i < N_CHAR; i++)
            {
                freq[i] = 1;
                son[i] = i + T;
                prnt[i + T] = i;
            }
            i = 0; j = N_CHAR;
            while (j <= R)
            {
                freq[j] = freq[i] + freq[i + 1];
                son[j] = i;
                prnt[i] = prnt[i + 1] = j;
                i += 2; j++;
            }
            freq[T] = 0xffff;
            prnt[R] = 0;
        }

        /* reconstruction of tree */

        private static void reconst()
        {
            int i, j, k;
            uint f, l;

            /* collect leaf nodes in the first half of the table */
            /* and replace the freq by (freq + 1) / 2. */
            j = 0;
            for (i = 0; i < T; i++)
            {
                if (son[i] >= T)
                {
                    freq[j] = (freq[i] + 1) / 2;
                    son[j] = son[i];
                    j++;
                }
            }
            /* begin constructing tree by connecting sons */
            for (i = 0, j = N_CHAR; j < T; i += 2, j++)
            {
                k = i + 1;
                f = freq[j] = freq[i] + freq[k];
                for (k = j - 1; f < freq[k]; k--) ;
                k++;
                l = (uint)((j - k) * 2);
                Array.Copy(freq, k, freq, k + 1, l);
                freq[k] = f;
                Array.Copy(son, k, son, k + 1, l);
                son[k] = i;
            }
            /* connect prnt */
            for (i = 0; i < T; i++)
            {
                if ((k = son[i]) >= T)
                {
                    prnt[k] = i;
                }
                else
                {
                    prnt[k] = prnt[k + 1] = i;
                }
            }
        }

        /* increment frequency of given code by one, and update tree */

        private static void update(int c)
        {
            int i, j, k, l;

            if (freq[R] == MAX_FREQ)
            {
                reconst();
            }
            c = prnt[c + T];
            do
            {
                k = (int)(++freq[c]);

                /* if the order is disturbed, exchange nodes */
                if ((uint)k > freq[l = c + 1])
                {
                    while ((uint)k > freq[++l]) ;
                    l--;
                    freq[c] = freq[l];
                    freq[l] = (uint)k;

                    i = son[c];
                    prnt[i] = l;
                    if (i < T) prnt[i + 1] = l;

                    j = son[l];
                    son[l] = i;

                    prnt[j] = c;
                    if (j < T) prnt[j + 1] = c;
                    son[c] = j;

                    c = l;
                }
            } while ((c = prnt[c]) != 0); /* repeat up to root */
        }

        private static void EncodeChar(uint c)
        {
            uint i;
            int j, k;

            i = 0;
            j = 0;
            k = prnt[c + T];

            /* travel from leaf to root */
            do
            {
                i >>= 1;

                /* if node's address is odd-numbered, choose bigger brother node */
                if ((k & 1) == 1) i += 0x8000;

                j++;
            } while ((k = prnt[k]) != R);
            Putcode(j, i);
            update((int)c);
        }

        private static void EncodePosition(uint c)
        {
            uint i;

            /* output upper 6 bits by table lookup */
            i = c >> 6;
            Putcode(p_len[i], (uint)p_code[i] << 8);

            /* output lower 6 bits verbatim */
            Putcode(6, (c & 0x3f) << 10);
        }

        private static void EncodeEnd()
        {
            if (putlen != 0)
            {
                outfile.Write((byte)(putbuf >> 8));
                codesize++;
            }
        }

        private static int DecodeChar()
        {
            uint c;

            c = (uint)son[R];

            /* travel from root to leaf, */
            /* choosing the smaller child node (son[]) if the read bit is 0, */
            /* the bigger (son[]+1} if 1 */
            while (c < T)
            {
                c += (uint)GetBit();
                c = (uint)son[c];
            }
            c -= T;
            update((int)c);
            return (int)c;
        }

        private static int DecodePosition()
        {
            uint i, j, c;

            /* recover upper 6 bits from table */
            i = (uint)GetByte();
            c = (uint)d_code[i] << 6;
            j = d_len[i];

            /* read lower 6 bits verbatim */
            j -= 2;
            while (j-- != 0)
            {
                i = (uint)((i << 1) + GetBit());
            }
            return (int)(c | (i & 0x3f));
        }

        /* compression */

        public static void CompressData(byte[] Input, out byte[] Output)
        {
            Stream In = new MemoryStream(Input);
            Stream Out = new MemoryStream();
            CompressData(In, Out);
            Output = new byte[outfile.BaseStream.Length];
            outfile.Seek(0, SeekOrigin.Begin);
            outfile.BaseStream.Read(Output, 0, (int)outfile.BaseStream.Length);
            In.Close();
            Out.Close();
        }

        public static void CompressData(Stream Input, Stream Output)  /* compression */
        {
            int i, c, len, r, s, last_match_length;
            int dataLength = (int)Input.Length;

            Clear();

            infile = new BinaryReader(Input);
            outfile = new BinaryWriter(Output);

            textsize = 0;           /* rewind and re-read */
            StartHuff();
            InitTree();
            s = 0;
            r = N - F;
            for (i = s; i < r; i++)
                text_buf[i] = 0x20;
            for (len = 0; len < F && infile.BaseStream.Position < dataLength; len++)
            {
                c = infile.ReadByte();
                text_buf[r + len] = (byte)c;
            }
            textsize = len;
            for (i = 1; i <= F; i++)
                InsertNode(r - i);
            InsertNode(r);

            do
            {
                if (match_length > len)
                    match_length = len;
                if (match_length <= THRESHOLD)
                {
                    match_length = 1;
                    EncodeChar(text_buf[r]);
                }
                else
                {
                    EncodeChar((uint)(255 - THRESHOLD + match_length));
                    EncodePosition((uint)(match_position));
                }
                last_match_length = match_length;
                for (i = 0; i < last_match_length && infile.BaseStream.Position < dataLength; i++)
                {
                    c = infile.ReadByte();
                    DeleteNode(s);
                    text_buf[s] = (byte)c;
                    if (s < F - 1)
                        text_buf[s + N] = (byte)c;
                    s = (s + 1) & (N - 1);
                    r = (r + 1) & (N - 1);
                    InsertNode(r);
                }
                textsize += i;

                while (i++ < last_match_length)
                {
                    DeleteNode(s);
                    s = (s + 1) & (N - 1);
                    r = (r + 1) & (N - 1);
                    if (--len != 0) InsertNode(r);
                }
            } while (len > 0);
            EncodeEnd();
        }

        public static void DecompressData(byte[] Input, int InOffset, out byte[] Output, int decodedSize)
        {
            Stream In = new MemoryStream(Input);
            In.Seek(InOffset, SeekOrigin.Begin);
            Output = new byte[decodedSize];
            Stream Out = new MemoryStream(Output);
            DecompressData(In, Out, decodedSize);
            outfile.Seek(0, SeekOrigin.Begin);
            outfile.BaseStream.Read(Output, 0, (int)outfile.BaseStream.Length);
            In.Close();
            Out.Close();
        }

        public static void DecompressData(Stream Input, Stream Output, int decodedSize)  /* recover */
        {
            int i, j, k, r, c;
            uint count;

            Clear();

            infile = new BinaryReader(Input);
            outfile = new BinaryWriter(Output);

            StartHuff();
            for (i = 0; i < N - F; i++)
                text_buf[i] = 0x20;
            r = N - F;
            for (count = 0; count < decodedSize;)
            {
                c = DecodeChar();
                if (c < 256)
                {
                    outfile.Write((byte)c);
                    text_buf[r++] = (byte)c;
                    r &= (N - 1);
                    count++;
                }
                else
                {
                    i = (r - DecodePosition() - 1) & (N - 1);
                    j = c - 255 + THRESHOLD;
                    for (k = 0; k < j; k++)
                    {
                        c = text_buf[(i + k) & (N - 1)];
                        outfile.Write((byte)c);
                        text_buf[r++] = (byte)c;
                        r &= (N - 1);
                        count++;
                    }
                }
            }
        }
    }
}