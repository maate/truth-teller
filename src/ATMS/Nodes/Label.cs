using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M8.ATMS.Nodes {
  /// <summary>
  ///   A label of a node comprises environments where the node holds in.
  /// </summary>
  public class Label : HashSet<Environment> {

    public Label() {
    }

    public Label( IEnumerable<Environment> environments ) {
      foreach ( Environment environment in environments ) {
        Add( environment );
      }
    }

    public static Label NoEnvironment {
      get {
        return new Label();
      }
    }

    public static Label EmptyEnvironment {
      get {
        return new Label { new Environment() };
      }
    }

    /// <summary>
    ///   Creates a new label given a list of <see cref="Environment" />
    /// </summary>
    public static Label Create( params Environment[] labels ) {
      return Create( labels.AsEnumerable() );
    }

    /// <summary>
    ///   Creates a new label given a list of <see cref="Environment" />
    /// </summary>
    public static Label Create( IEnumerable<Environment> environments ) {
      var l = new Label();
      var flattenedLabels = new Label();

      foreach ( Environment label in environments ) {
        flattenedLabels.Add( label );
      }
      l = flattenedLabels;
      return l;
    }

    /// <summary>
    ///   Conjoins a set of labels with the current instance. If only one label is received, it is treated as a disjunction.
    /// </summary>
    /// <param name="labels"></param>
    public void Conjoin( Conjunction<Label> labels ) {
      if ( !labels.Any() || labels.All( l => !l.Any() ) ) {
        return; // nothing to join with
      }
      foreach ( Label label in labels ) {
        Duplicate( label.Count );
        if ( Count == 0 ) {
          foreach ( Environment environment in label ) {
            Add( environment.Clone() );
          }
          continue;
        }

        int lblCounter = 0;
        while ( lblCounter < Count ) {
          foreach ( Environment v in label ) {
            this.ElementAt( lblCounter ).UnionWith( v );
            lblCounter++;
          }
        }
      }
    }

    public void Disjoin( params Label[] labels ) {
      foreach ( Label label in labels ) {
        foreach ( Environment env in label ) {
          Add( env.Clone() );
        }
      }
    }

    /// <summary>
    ///   Joins the environment of many labels into one label
    /// </summary>
    public static Label Join( IEnumerable<Label> labels ) {
      var flattenedLabel = new Label();

      foreach ( Label label in labels ) {
        foreach ( Environment item in label ) {
          flattenedLabel.Add( item );
        }
      }

      return flattenedLabel;
    }

    public override bool Equals( object obj ) {
      var other = obj as Label;
      if ( other == null ) {
        return false;
      }
      if ( ReferenceEquals( this, other ) ) {
        return true;
      }
      if ( Count != other.Count ) {
        return false;
      }
      List<Environment> others = other.OrderBy( h => h.Comparer ).ToList();
      List<Environment> self = this.OrderBy( h => h.Comparer ).ToList();
      for ( int i = 0; i < other.Count; i++ ) {
        if ( !self[i].Equals( others[i] ) ) {
          return false;
        }
      }
      return true;
    }

    /// <summary>
    ///   Duplicates the environments to prepare for conjunction
    /// </summary>
    /// <param name="count">The factor to grow the environment list with. If '1', then no changes will occur.</param>
    private void Duplicate( int count ) {
      if ( count < 1 ) {
        throw new ArgumentException( "Count must be larger than 0", "count" );
      }
      var environments = new HashSet<Environment>();
      for ( int i = 0; i < count - 1; i++ ) {
        foreach ( Environment v in this ) {
          environments.Add( v.Clone() );
        }
      }
      foreach ( Environment e in environments ) {
        Add( e );
      }
    }

    /// <summary>
    ///   Parses a string of the form { { A, B }, C } into a label
    /// </summary>
    public static Label Parse( string label ) {
      var l = new Label();
      int lvl = 0;
      string buffer = string.Empty;
      foreach ( char c in label ) {
        if ( c == ' ' ) {
          continue;
        }
        if ( c == '{' ) {
          if ( lvl == 1 ) {
            l.Add( new Environment() );
          }
          else if ( lvl > 2 ) {
            throw new FormatException( "A Label can contain only two levels of nesting using curly braces" );
          }
          lvl++;
        }
        else if ( c == '}' ) {
          if ( lvl == 2 ) {
            l.ElementAt( l.Count - 1 ).AddId( buffer );
            buffer = string.Empty;
          }
          lvl--;
        }
        else if ( c == ',' ) {
          if ( buffer == string.Empty && lvl == 2 ) {
            throw new FormatException( "An item in a non-empty set cannot be empty" );
          }
          if ( lvl == 2 ) {
            l.ElementAt( l.Count - 1 ).AddId( buffer );
            buffer = string.Empty;
          }
        }
        else {
          buffer += c;
        }
      }
      if ( lvl != 0 ) {
        throw new FormatException( "A label must contain the exact same number of opening and closing braces" );
      }
      return l;
    }

    public override string ToString() {
      var aStr = new StringBuilder();
      aStr.Append( "{" );
      foreach ( Environment set in this ) {
        aStr.Append( " {" );
        aStr.Append( String.Join( ",", set ) );
        aStr.Append( "}, " );
      }
      if ( aStr.Length > 2 ) {
        aStr = aStr.Remove( aStr.Length - 2, 1 );
      }
      aStr.Append( "}" );
      return aStr.ToString();
    }

    /// <summary>
    ///   Removes environments which are supersets of other environments.
    ///   For example, if {{D}} is a justification and {{D, E}} is a justification,
    ///   then {{D, E}} can be removed because it is subsumed by {{D}}.
    /// </summary>
    internal void RemoveSubsumed() {
      if ( Count == 1 ) {
        return;
      }
      var subsumed = new List<Environment>();
      foreach ( Environment env in this ) {
        bool isSubsumed = false;
        foreach ( Environment other in this.Except( new[] { env } ) ) {
          if ( env.IsProperSupersetOf( other ) ) {
            isSubsumed = true;
          }
        }
        if ( isSubsumed ) {
          subsumed.Add( env );
        }
      }
      foreach ( Environment env in subsumed ) {
        Remove( env );
      }
    }

    public void FilterNogoodSets( Label nogood ) {
      RemoveWhere( env => nogood.Any( set => env.Equals( set ) || env.IsProperSupersetOf( set ) ) );
    }

  }
}