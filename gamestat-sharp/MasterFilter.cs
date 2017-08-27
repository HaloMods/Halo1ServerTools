/*
	Copyright (c) 2004 Cory Nelson

	Permission is hereby granted, free of charge, to any person obtaining
	a copy of this software and associated documentation files (the
	"Software"), to deal in the Software without restriction, including
	without limitation the rights to use, copy, modify, merge, publish,
	distribute, sublicense, and/or sell copies of the Software, and to
	permit persons to whom the Software is furnished to do so, subject to
	the following conditions:

	The above copyright notice and this permission notice shall be included
	in all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
	EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
	MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
	IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
	CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
	TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
	SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;

namespace GameStat {
	/// <summary>
	/// Defines master filters for the <see cref="GameStat.ServerInfo.QueryMaster"/> method.
	/// </summary>
	public enum MasterFilter : ushort {
		/// <summary>
		/// Retrieve the default server list.
		/// </summary>
		None=0,
		/// <summary>
		/// Retrieve full servers.
		/// </summary>
		Full=1,
		/// <summary>
		/// Retrieve servers that aren't full.
		/// </summary>
		NotFull=2,
		/// <summary>
		/// Retrieve servers without players.
		/// </summary>
		Empty=4,
		/// <summary>
		/// Retrieve servers that have players.
		/// </summary>
		NotEmpty=8,
		/// <summary>
		/// Retrieve password-protected servers.
		/// </summary>
		Password=16,
		/// <summary>
		/// Retrieve servers that dont require a password.
		/// </summary>
		NoPassword=32
	}
}
