using System;

namespace M8.ATMS.Nodes {
  public abstract class Datum: IEquatable<string> {
    protected Datum( string id ) {
      Id = id;
    }

    public string Id { get; private set; }

    public bool Equals( string other ) {
      return Id == other;
    }

    public override string ToString() {
      return Id;
    }

    public override int GetHashCode() {
      return Id.GetHashCode();
    }

    public static implicit operator string( Datum d ) {
      return d.Id;
    }

    public override bool Equals( object obj ) {
      var otherDatum = obj as Datum;
      if ( otherDatum != null ) {
        return Id == otherDatum.Id;
      }
      var otherString = obj as String;
      if ( otherString != null ) {
        return Equals( otherString );
      }
      return false;
    }
  }
}