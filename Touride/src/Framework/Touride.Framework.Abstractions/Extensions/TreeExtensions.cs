namespace Touride.Framework.Abstractions.Extensions
{
    /// <summary>
    /// Tree Data Structure for storing hierarchical data
    /// </summary>
    public static class TreeExtensions
    {
        /// <summary> Tree Node veri yapısı için generic interface</summary>
        /// <typeparam name="T"></typeparam>
        public interface ITree<T>
        {
            T Data { get; }
            ITree<T> Parent { get; }
            ICollection<ITree<T>> Children { get; }
            bool IsRoot { get; }
            bool IsLeaf { get; }
            int Level { get; }
        }

        /// <summary> Flatten tree to plain list of nodes </summary>
        public static List<TNode> Flatten<TNode>(this List<TNode> nodes, Func<TNode, List<TNode>> childrenSelector)
        {
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));
            var tNodes = nodes.ToList();
            return tNodes.SelectMany(c => childrenSelector(c).Flatten(childrenSelector)).Concat(tNodes).ToList();
        }
        /// <summary> Converts given list to tree. </summary>
        /// <typeparam name="T">Custom data type to associate with tree node.</typeparam>
        /// <param name="items">The collection items.</param>
        /// <param name="parentSelector">Expression to select parent.</param>
        public static ITree<T> ToTree<T>(this List<T> items, Func<T, T, bool> parentSelector)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            var lookup = items.ToLookup(item => items.FirstOrDefault(parent => parentSelector(parent, item)),
                child => child);
            return Tree<T>.FromLookup(lookup!);
        }
        /// <summary> Internal implementation of <see cref="ITree{T}" /></summary>
        /// <typeparam name="T">Custom data type to associate with tree node.</typeparam>
        internal class Tree<T> : ITree<T>
        {
            public T Data { get; }
            public ITree<T> Parent { get; private init; } = null!;
            public ICollection<ITree<T>> Children { get; }
            public bool IsRoot => false;
            public bool IsLeaf => Children.Count == 0;
            public int Level => IsRoot ? 0 : Parent.Level + 1;
            private Tree(T data)
            {
                Children = new LinkedList<ITree<T>>();
                Data = data;
            }
            public static Tree<T> FromLookup(ILookup<T, T> lookup)
            {
                var rootData = lookup.Count == 1 ? lookup.First().Key : default(T);
                var root = new Tree<T>(rootData!);
                root.LoadChildren(lookup);
                return root;
            }
            private void LoadChildren(ILookup<T, T> lookup)
            {
                foreach (var data in lookup[Data])
                {
                    var child = new Tree<T>(data) { Parent = this };
                    Children.Add(child);
                    child.LoadChildren(lookup);
                }
            }
        }
    }
}