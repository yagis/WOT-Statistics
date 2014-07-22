using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WOTStatistics.Core
{
    /*
   This program is part of BruNet, a library for the creation of efficient overlay
   networks.
   Copyright (C) 2005  University of California

   This program is free software; you can redistribute it and/or
   modify it under the terms of the GNU General Public License
   as published by the Free Software Foundation; either version 2
   of the License, or (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software
   Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
   */

    using System;

    /// <summary>
    /// This class to do base 32 coversions to and from byte arrays per the official RFC 4648 spec (http://tools.ietf.org/html/rfc4648).
    /// </summary>
    public sealed class Base32
    {
        private static readonly char[] alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H',
                                                           'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P',
                                                           'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X',
                                                           'Y', 'Z', '2', '3', '4', '5', '6', '7' };
        private static readonly char padding = '=';

        private Base32()
        {
            //DO NOT INSTANTIATE
        }

        /// <summary>
        /// Get the number for the character.  For example, A->0, B->1, etc....
        /// </summary>
        /// <param name="c">character to look up its number in the base 32 scheme</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">c is not valid</exception>
        private static int ValueFor(char c)
        {
            //Allow lower cases letters
            if (('a' <= c) && (c <= 'z'))
                return (c - 'a');
            else if (('A' <= c) && (c <= 'Z'))
                return (c - 'A');
            else if (('2' <= c) && (c <= '7'))
                return c - '2' + 26;

            throw new ArgumentOutOfRangeException("c", c, "Not a valid Base32 character");
        }

        /// <summary>
        /// By default, we encode WITH padding to be standard compliant.
        /// </summary>
        /// <param name="binary"></param>
        /// <returns></returns>
        public static string Encode(byte[] binary)
        {
            return Encode(binary, 0, binary.Length, true);
        }

        /// <summary>
        /// Get the base 32 value of the given byte array with or without padding.
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="pad">if true, add padding characters to make output length a multiple of 8</param>
        /// <returns></returns>
        public static string Encode(byte[] binary, bool pad)
        {
            return Encode(binary, 0, binary.Length, pad);
        }

        /// <summary>
        /// Get the base 32 value of the given byte array (or subset of the given byte array) with or without padding.
        /// </summary>
        /// <param name="binary"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="pad"></param>
        /// <returns></returns>
        public static string Encode(byte[] binary, int offset, int length, bool pad)
        {
            int enc_length = 8 * (length / 5);
            int pad_length = 0;

            switch (length % 5)
            {
                case 1:
                    pad_length = 6;
                    break;
                case 2:
                    pad_length = 4;
                    break;
                case 3:
                    pad_length = 3;
                    break;
                case 4:
                    pad_length = 1;
                    break;
                default:
                    pad_length = 0;
                    break;
            }

            if (pad_length > 0)
            {
                if (pad)
                    //Add a full block
                    enc_length += 8;
                else
                    //Just add the chars we need :
                    enc_length += (8 - pad_length);
            }

            char[] encoded = new char[enc_length];

            //Here are all the full blocks :
            //This is the number of full blocks :
            int blocks = length / 5;
            for (int block = 0; block < blocks; block++)
                EncodeBlock(encoded, 8 * block, binary, offset + 5 * block, 5);

            //Here is one last partial block
            EncodeBlock(encoded, 8 * blocks, binary, offset + 5 * blocks, length % 5);

            //Add the padding at the end
            if (pad)
                for (int i = 0; i < pad_length; i++)
                    encoded[enc_length - i - 1] = padding;

            return new string(encoded);
        }

        private static void EncodeBlock(char[] ascii_out, int ascii_off, byte[] bin, int offset, int block_length)
        {
            //The easiest thing is just to do this by hand :
            int idx = 0;
            switch (block_length)
            {
                case 5:
                    idx |= bin[offset + 4] & 0x1F;
                    ascii_out[ascii_off + 7] = alphabet[idx];
                    idx = 0;
                    idx |= (bin[offset + 4] & 0xE0) >> 5;
                    goto case 4;
                case 4:
                    idx |= (bin[offset + 3] & 0x03) << 3;
                    ascii_out[ascii_off + 6] = alphabet[idx];
                    idx = 0;
                    idx |= (bin[offset + 3] & 0x7C) >> 2;
                    ascii_out[ascii_off + 5] = alphabet[idx];
                    idx = 0;
                    idx |= (bin[offset + 3] & 0x80) >> 7;
                    goto case 3;
                case 3:
                    idx |= (bin[offset + 2] & 0x0F) << 1;
                    ascii_out[ascii_off + 4] = alphabet[idx];
                    idx = 0;
                    idx |= (bin[offset + 2] & 0xF0) >> 4;
                    goto case 2;
                case 2:
                    idx |= (bin[offset + 1] & 0x01) << 4;
                    ascii_out[ascii_off + 3] = alphabet[idx];
                    idx = 0;
                    idx |= (bin[offset + 1] & 0x3E) >> 1;
                    ascii_out[ascii_off + 2] = alphabet[idx];
                    idx = 0;
                    idx |= (bin[offset + 1] & 0xC0) >> 6;
                    goto case 1;
                case 1:
                    idx |= (bin[offset] & 0x7) << 2;
                    ascii_out[ascii_off + 1] = alphabet[idx];
                    idx = 0;
                    idx |= bin[offset] >> 3;
                    ascii_out[ascii_off] = alphabet[idx];
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get the base 32 byte array representation of the given string.
        /// </summary>
        /// <param name="ascii"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">if there are any characters in the ascii that are not valid base32 chars before the first padding character :  '=' All characters after the first padding character are ignored.</exception>
        public static byte[] Decode(string ascii)
        {
            //Now find the number of non-pad chars in the last block
            int pad_pos = ascii.IndexOf(padding);
            if (pad_pos == -1)
                //consider the end of the string to be the position of the '='
                pad_pos = ascii.Length;

            //These are full blocks :
            int data_length = 5 * (pad_pos / 8);
            switch (pad_pos % 8)
            {
                case 7:
                    data_length += 4;
                    break;
                case 6:
                    goto case 5;
                case 5:
                    data_length += 3;
                    break;
                case 4:
                    goto case 3;
                case 3:
                    data_length += 2;
                    break;
                case 2:
                    data_length += 1;
                    break;
                case 1:
                    goto default;
                default:
                    break;
            }

            byte[] binary = new byte[data_length];
            int blocks = pad_pos / 8;
            for (int block = 0; block < blocks; block++)
                DecodeBlock(binary, 5 * block, ascii, 8 * block, 8);

            DecodeBlock(binary, 5 * blocks, ascii, 8 * blocks, pad_pos % 8);

            return binary;
        }

        private static void DecodeBlock(byte[] data, int offset, string ascii, int a_off, int encl)
        {
            //We just do this by hand :
            int val = 0;
            switch (encl)
            {
                case 8:
                    val |= ValueFor(ascii[a_off + 7]);
                    val |= (ValueFor(ascii[a_off + 6]) & 0x7) << 5;
                    data[offset + 4] = (byte)val;
                    val = 0;
                    goto case 7;
                case 7:
                    val |= ValueFor(ascii[a_off + 6]) >> 3;
                    val |= ValueFor(ascii[a_off + 5]) << 2;
                    val |= (ValueFor(ascii[a_off + 4]) & 1) << 7;
                    data[offset + 3] = (byte)val;
                    val = 0;
                    goto case 5;
                case 6:
                    goto case 5;
                case 5:
                    val |= ValueFor(ascii[a_off + 4]) >> 1;
                    val |= (ValueFor(ascii[a_off + 3]) & 0xF) << 4;
                    data[offset + 2] = (byte)val;
                    val = 0;
                    goto case 4;
                case 4:
                    val |= ValueFor(ascii[a_off + 3]) >> 4;
                    val |= ValueFor(ascii[a_off + 2]) << 1;
                    val |= (ValueFor(ascii[a_off + 1]) & 0x03) << 6;
                    data[offset + 1] = (byte)val;
                    val = 0;
                    goto case 2;
                case 3:
                    goto case 2;
                case 2:
                    val |= ValueFor(ascii[a_off + 1]) >> 2;
                    val |= ValueFor(ascii[a_off]) << 3;
                    data[offset] = (byte)val;
                    goto case 1;
                case 1:
                    goto default;
                default:
                    break;
            }
        }
    }
}
