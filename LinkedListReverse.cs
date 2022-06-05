using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benchmarking;

/*
|                   Method |      Mean |    Error |   StdDev | Allocated |
|------------------------- |----------:|---------:|---------:|----------:|
|          ReverseWithList |  91.90 ns | 0.485 ns | 0.453 ns |         - | THE WINNER
|         ReverseWithStack |  96.49 ns | 0.649 ns | 0.542 ns |         - |
| ReverseWithoutCollection | 102.18 ns | 0.738 ns | 0.690 ns |         - |
*/

/*
|                   Method |     Mean |   Error |  StdDev | Allocated |
|------------------------- |---------:|--------:|--------:|----------:|
|          ReverseWithList | 111.1 ns | 0.48 ns | 0.40 ns |         - |
|         ReverseWithStack | 100.3 ns | 0.51 ns | 0.45 ns |         - | THE WINNER
| ReverseWithoutCollection | 102.1 ns | 0.37 ns | 0.31 ns |         - |
*/

[MemoryDiagnoser]
public class LinkedListReverse
{
    const int ITERATIONS = 10;

    public class ListNode<T>
    {
        public T value { get; set; }
        public ListNode<T> next { get; set; }
    }

    private static class IntLinkedListCreator
    {
        public static ListNode<int> GetListNode(string text)
        {
            int indexOfStart = text.IndexOf('[');
            int indexOfEnd = text.IndexOf(']');
            if (indexOfStart == -1 || indexOfEnd == -1 || indexOfStart >= indexOfEnd)
                throw new ArgumentException($"Variable {nameof(text)} does not start with \"[\" or doesn not ends with \"]\"");

            string substring = text.Substring(indexOfStart + 1, indexOfEnd - 1) + ",";

            if (String.IsNullOrEmpty(substring) || substring == ",") return new ListNode<int>();

            ListNode<int> head = null;
            ListNode<int> current = null;

            var subPart = new StringBuilder(4);

            foreach (var c in substring)
            {
                if (c != ',')
                {
                    if (!Char.IsWhiteSpace(c)) subPart.Append(c);
                }
                else
                {
                    int parsedValue = Int32.Parse(subPart.ToString().Trim());
                    subPart.Clear();

                    if (head is null)
                    {
                        head = new ListNode<int>();
                        current = head;
                        current.value = parsedValue;
                    }
                    else
                    {
                        var newNode = new ListNode<int>
                        {
                            value = parsedValue
                        };

                        current.next = newNode;
                        current = current.next;
                    }
                }
            }

            if (head is null) head = new ListNode<int>();

            return head;
        }
    }

    private readonly static ListNode<int> l1 = IntLinkedListCreator.GetListNode("[1]");
    private readonly static ListNode<int> l2 = IntLinkedListCreator.GetListNode("[1, 2, 3, 4]");
    private readonly static ListNode<int> l3 = IntLinkedListCreator.GetListNode("[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40]");

    public ListNode<int> _dummyNode;

    [Benchmark]
    public void ReverseWithList()
    {
        for (int i = 0; i < ITERATIONS; i++)
        {
            _dummyNode = S_Reverse_WithList(l1);
            _dummyNode = S_Reverse_WithList(l2);
            _dummyNode = S_Reverse_WithList(l3);
        }
    }

    [Benchmark]
    public void ReverseWithStack()
    {
        for (int i = 0; i < ITERATIONS; i++)
        {
            _dummyNode = S_Revers_WithStack(l1);
            _dummyNode = S_Revers_WithStack(l2);
            _dummyNode = S_Revers_WithStack(l3);
        }
    }

    [Benchmark]
    public void ReverseWithoutCollection()
    {
        for (int i = 0; i < ITERATIONS; i++)
        {
            _dummyNode = S_Reverse_WithoutColections(l1);
            _dummyNode = S_Reverse_WithoutColections(l2);
            _dummyNode = S_Reverse_WithoutColections(l3);
        }
    }


    private ListNode<int> S_Reverse_WithoutColections(ListNode<int> l)
    {
        if (l is null) return null;
        if (l.next is null) return l;

        ListNode<int> curr = l;
        ListNode<int> prev = null;

        while (curr is not null)
        {
            var next = curr.next;
            curr.next = prev;
            prev = curr;
            curr = next;
        }

        return prev;
    }

    private ListNode<int> S_Revers_WithStack(ListNode<int> l)
    {
        if (l is null) return null;
        if (l.next is null) return l;

        Stack<ListNode<int>> listStack = new Stack<ListNode<int>>();

        for (var n = l; n is not null; n = n.next)
        {
            listStack.Push(n);
        }

        ListNode<int> head = null;
        ListNode<int> prev = null;

        int count = listStack.Count;

        for (int i = 0; i < count; i++)
        {
            if (head is null)
            {
                head = listStack.Pop();
                prev = head;
            }
            else
            {
                var n = listStack.Pop();
                prev.next = n;
                prev = prev.next;
            }
        }
        prev.next = null;

        return head;
    }

    private ListNode<int> S_Reverse_WithList(ListNode<int> l)
    {
        if (l is null) return null;
        if (l.next is null) return l;

        List<ListNode<int>> listList = new List<ListNode<int>>();

        for (ListNode<int> n = l; n != null; n = n.next)
        {
            listList.Add(n);
        }

        int count = listList.Count;
        ListNode<int> newHead = listList[count - 1];

        for (int i = count - 1; i >= 1; i--)
        {
            listList[i].next = listList[i - 1];
        }
        listList[0].next = null;

        return newHead;
    }

}
