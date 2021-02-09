using DataStructure;
using System;
using System.IO;
using System.Text;

namespace DataStrucrture
{
    public class ListRandom
    {
        public int Length { get; protected set; }

        private Random _random;
        private ListNode _root;
        private ListNode _tale;

        public ListRandom() : base()
        {
            _random = new Random();
        }

        public void Add(string value)
        {
            if (Length > 0)
            {
                ListNode tmp = _tale;
                tmp.Next = new ListNode(value);
                tmp.Next.Previous = tmp;
                _tale = tmp.Next;
                Length++;
            }
            else
            {
                _root = new ListNode(value);
                _tale = _root;
                Length++;
            }
            _tale.Random = GetNodeByIndex(_random.Next(0, Length));            
        }

        public void Serialize(Stream s)
        {
            if (s.CanWrite)
            {
                var sw = new StreamWriter(s);
                sw.Write(ToJSON());
                sw.Flush();
            }
        }

        public void Deserialize(Stream s)
        {
            if (s.CanRead)
            {
                var sr = new StreamReader(s);
                string json = sr.ReadToEnd();
                var DTOs = GetNodeDTOsByJSON(json);

                _root = null;
                Length = 0;
                foreach (var DTO in DTOs)
                {
                    Add(DTO.Value);
                }

                ListNode tmp = _root;
                foreach (var DTO in DTOs)
                {
                    tmp.Random = GetNodeByIndex(DTO.Random);
                    tmp = tmp.Next;
                }
            }
        }

        public string ToJSON()
        {
            var sb = new StringBuilder();
            ListNode tmp = _root;
            sb.Append("[");

            for (int i = 0; i < Length; i++)
            {
                sb.Append("{\"Value\":" + $"\"{tmp.Value}\"");
                sb.Append(",\"Previous\":" + ((i - 1) < 0 ? "null" : $"{(i - 1)}"));
                sb.Append(",\"Next\":" + ((i + 1) == Length ? "null" : $"{(i + 1)}"));
                sb.Append(",\"Random\":" + TryGetIndexByNode(tmp.Random) + "},");
                tmp = tmp.Next;
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }

        private ListNodeDTO[] GetNodeDTOsByJSON(string json)
        {
            json = json.Trim('[', ']');
            json = json.Replace("\"", "");
            var jObjs = json.Split("},");
            var DTOs = new ListNodeDTO[jObjs.Length];

            for (int i = 0; i < jObjs.Length; i++)
            {
                var properties = jObjs[i].Trim('{', '}').Split(',');
                ListNodeDTO node = new ListNodeDTO();
                foreach (var property in properties)
                {
                    switch (property.Split(':')[0])
                    {
                        case "Value":
                            node.Value = property.Split(':')[1];
                            break;
                        case "Random":
                            node.Random = Convert.ToInt32(property.Split(':')[1]);
                            break;
                    }
                }
                DTOs[i] = node;
            }
            return DTOs;
        }

        private int? TryGetIndexByNode(ListNode node)
        {
            ListNode tmp = _root;
            for (int i = 0; i < Length; i++)
            {
                if (node == tmp)
                {
                    return i;
                }
                tmp = tmp.Next;
            }
            return null;
        }

        private ListNode GetNodeByIndex(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new IndexOutOfRangeException();
            }
            ListNode current;

            if (index < Length / 2)
            {
                current = _root;

                for (int i = 1; i <= index; i++)
                {
                    current = current.Next;
                }
            }
            else
            {
                current = _tale;

                for (int i = Length - 1; i > index; i--)
                {
                    current = current.Previous;
                }
            }
            return current;
        }

    }
}
