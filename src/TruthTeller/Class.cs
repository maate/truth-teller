using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using M8.ATMS.Nodes;

namespace M8.ATMS {
  /// <summary>
  ///   A class is used to group nodes. Zero or more nodes can belong to a class.
  /// </summary>
  /// <remarks>
  ///   De Kleer in An Assumption-based TMS:
  ///
  ///   "It is often convenient to group nodes together into sets and treat each member
  ///   identically. The architecture incorporates a notion of node set, or class. The
  ///   node-class framework is not logically necessary, but it is a minor extension
  ///   which greatly simplifies problem-solver-ATMS interactions. Dc Kleer, in [81,
  ///   utilizes this framework to encode complex inference rules.
  ///
  ///   The node-class organization is intended to he very general. A node can be a
  ///   member of any number of classes (possibly none), and a class can have any
  ///   number of members (possibly none). The membership of a node in a class has
  ///   no relation to whether it holds in any environment. Unless a class has been
  ///   specifically closed, nodes can be added to it at any time. It is not possible to
  ///   remove a node from a class."
  /// </remarks>
  public class Class : ISet<Node> {
    private readonly HashSet<Node> _nodeSet;

    private readonly Dictionary<string, Node> _nodes;

    private bool _open;

    public Class( string id ) {
      Id = id;
      _nodes = new Dictionary<string, Node>();
      _nodeSet = new HashSet<Node>();
      _open = true;
    }

    public string Id { get; private set; }

    public IEnumerator<Node> GetEnumerator() {
      return new ReadOnlyCollection<Node>( _nodeSet.ToList() ).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    void ICollection<Node>.Add( Node item ) {
      ( (ISet<Node>)this ).Add( item );
    }

    public void Clear() {
      throw new InvalidOperationException(
        "According to the specification, it is not possible to remove a node from a class" );
    }

    public bool Contains( Node item ) {
      return Contains( item.Id );
    }

    public void CopyTo( Node[] array, int arrayIndex ) {
      _nodes.Values.CopyTo( array, arrayIndex );
    }

    public bool Remove( Node item ) {
      throw new InvalidOperationException(
        "According to the specification, it is not possible to remove a node from a class" );
    }

    public int Count {
      get {
        return _nodes.Count;
      }
    }

    public bool IsReadOnly {
      get {
        return false;
      }
    }

    public bool Add( Node item ) {
      if ( !_open ) {
        throw new InvalidOperationException( @"The class was closed by invoking the Close() method.
It is not possible to add a new node to the class." );
      }
      _nodes[item.Id] = item;
      return _nodeSet.Add( item );
    }

    public void ExceptWith( IEnumerable<Node> other ) {
      _nodeSet.ExceptWith( other );
    }

    public void IntersectWith( IEnumerable<Node> other ) {
      _nodeSet.IntersectWith( other );
    }

    public bool IsProperSubsetOf( IEnumerable<Node> other ) {
      return _nodeSet.IsProperSubsetOf( other );
    }

    public bool IsProperSupersetOf( IEnumerable<Node> other ) {
      return _nodeSet.IsProperSupersetOf( other );
    }

    public bool IsSubsetOf( IEnumerable<Node> other ) {
      return _nodeSet.IsSubsetOf( other );
    }

    public bool IsSupersetOf( IEnumerable<Node> other ) {
      return _nodeSet.IsSupersetOf( other );
    }

    public bool Overlaps( IEnumerable<Node> other ) {
      return _nodeSet.Overlaps( other );
    }

    public bool SetEquals( IEnumerable<Node> other ) {
      return _nodeSet.SetEquals( other );
    }

    public void SymmetricExceptWith( IEnumerable<Node> other ) {
      _nodeSet.SymmetricExceptWith( other );
    }

    public void UnionWith( IEnumerable<Node> other ) {
      _nodeSet.UnionWith( other );
    }

    /// <summary>
    ///   A closed class cannot be extended with additional nodes.
    /// </summary>
    public void Close() {
      _open = false;
    }

    public bool Contains( string nodeId ) {
      return _nodes.ContainsKey( nodeId );
    }
  }
}