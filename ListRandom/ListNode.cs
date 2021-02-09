using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructure
{
    public class ListNode
    {
        public string Value { get; set; }
        public ListNode Previous { get; set; }
        public ListNode Next { get; set; }
        public ListNode Random { get; set; }
        public ListNode(string value)
        {
            Value = value;
            Previous = null;
            Next = null;
        }
    }
}
