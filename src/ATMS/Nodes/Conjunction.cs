using System.Collections;
using System.Collections.Generic;

namespace M8.ATMS.Nodes {
  public class Conjunction<T> : IEnumerable<T> {
    public Conjunction() {
      Values = new HashSet<T>();
    }

    public Conjunction( IEnumerable<T> self ) {
      Values = new HashSet<T>( self );
    }

    public IEqualityComparer<T> Comparer {
      get {
        return Values.Comparer;
      }
    }

    public HashSet<T> Values { get; set; }

    public IEnumerator<T> GetEnumerator() {
      return Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }

    public override int GetHashCode() {
      return Values.GetHashCode();
    }

    public void Add( T item ) {
      Values.Add( item );
    }

    public bool IsProperSupersetOf( Conjunction<T> other ) {
      return Values.IsProperSupersetOf( other );
    }

    public void UnionWith( Conjunction<T> other ) {
      Values.UnionWith( other );
    }

    public Conjunction<T> Clone() {
      var conj = new Conjunction<T>();
      foreach ( T item in Values ) {
        conj.Add( item );
      }
      return conj;
    }

    public override bool Equals( object obj ) {
      var other = obj as Conjunction<T>;
      if ( other == null ) {
        return false;
      }
      return Values.SetEquals( other.Values );
    }

    public override string ToString() {
      string retval = string.Format( "{0}", string.Join( ", ", Values ) );
      return retval;
    }

    public void Clear() {
      Values.Clear();
    }
  }
}