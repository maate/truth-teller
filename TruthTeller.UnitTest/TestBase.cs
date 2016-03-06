using M8.TruthTeller.Nodes;

using NUnit.Framework;

namespace M8.TruthTeller.UnitTest {
  public abstract class TestBase {
    protected TruthTeller Informant;

    protected TruthRecorder Recorder;

    [SetUp]
    public void SetUp() {
      var tms = new TMS();
      Recorder = new TruthRecorder( tms );
      Informant = new TruthTeller( tms );
    }

    protected void SetupABr() {
      Recorder.Assume( new Node { Id = "A" } );
      Recorder.Assume( new Node { Id = "B" } );
      Recorder.Justify( new Node { Id = "r" } ).WithAntecedents( "A" );
      Recorder.Justify( new Node { Id = "r" } ).WithAntecedents( "B" );
    }

    protected void SetupABrConjoint() {
      Recorder.Assume( new Node { Id = "A" } );
      Recorder.Assume( new Node { Id = "B" } );
      Recorder.Justify( new Node { Id = "r" } ).WithAntecedents( "A", "B" );
    }

    protected void Setupg() {
      Recorder.Justify( new Node { Id = "g" } ).WithAntecedents( "r" );
    }

    protected void Setupl() {
      Recorder.Justify( new Node { Id = "l" } ).WithAntecedents( "D" );
    }

    protected void SetupD() {
      Recorder.Assume( new Node { Id = "D" } );
    }

    protected void Setupi() {
      Recorder.Justify( new Node { Id = "i" } ).WithAntecedents( "l" );
    }

    protected void Setupk() {
      Recorder.Justify( new Node { Id = "k" } ).WithAntecedents( "D" );
      Recorder.Justify( new Node { Id = "k" } ).WithAntecedents( "E" );
    }

    protected void SetupE() {
      Recorder.Assume( new Node { Id = "E" } );
    }

    protected void Setuph() {
      Recorder.Justify( new Node { Id = "h" } ).WithAntecedents( "g", "i" );
    }

    public void AssertLabel( string node, string label ) {
      Node n = Informant.GetNode( node );
      Assert.That( n.Label.ToString(), Is.EqualTo( Label.Parse( label ).ToString() ) );
    }
  }
}