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

using System.Collections;
using GameStat;

namespace GameStat.Collections {
	/// <summary>
	/// A strongly typed collection for holding server information.
	/// </summary>
	public class ServerCollection : System.Collections.CollectionBase {
		/// <summary>
		/// Adds an item to the ServerCollection.
		/// </summary>
		/// <param name="value">The Server to add to the ServerCollection</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(Server value) {
			return base.List.Add(value);
		}

		/// <summary>
		/// Removes the first occurrence of a specific Server from the ServerCollection.
		/// </summary>
		/// <param name="value">The Server to remove from the ServerCollection</param>
		public void Remove(Server value) {
			base.List.Remove(value);
		}

		/// <summary>
		/// Inserts an item to the ServerCollection at the specified position.
		/// </summary>
		/// <param name="index">The zero-based index at which value should be inserted.</param>
		/// <param name="value">The Server to insert into the ServerCollection.</param>
		public void Insert(int index, Server value) {
			base.List.Insert(index, value);
		}

		/// <summary>
		/// Determines whether the ServerCollection contains a specific value.
		/// </summary>
		/// <param name="value">The Server to locate in the ServerCollection.</param>
		/// <returns>true if the Server is found in the ServerCollection; otherwise, false.</returns>
		public bool Contains(Server value) {
			return base.List.Contains(value);
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		public Server this[int index] {
			get {
				return (Server)base.List[index];
			}
			set {
				base.List[index]=value;
			}
		}

		/// <summary>
		/// Creates a new instance of the ServerCollection class.
		/// </summary>
		public ServerCollection() {}
	}
}
