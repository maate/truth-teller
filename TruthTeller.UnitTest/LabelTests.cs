using System.Linq;

using M8.TruthTeller.Nodes;

using NUnit.Framework;

namespace M8.TruthTeller.UnitTest {
  [TestFixture]
  internal class LabelTests {
    [Test]
    [Ignore( "TODO" )]
    public void EqualityIgnoresEnvironmentOrder() {
      Label l1 = Label.Parse( "{{ A }, { B, C }}" );
      Label l2 = Label.Parse( "{{ B, C }, {A}}" );
      Assert.That( l1.Equals( l2 ) );
    }

    [Test]
    public void EqualityIgnoresSetOrder() {
      Label l1 = Label.Parse( "{{ A }, { B, C }}" );
      Label l2 = Label.Parse( "{{ A }, { C, B }}" );
      Assert.That( l1.Equals( l2 ) );
    }

    [Test]
    public void ParseConjunction() {
      string s = "{{ A, B }}";
      Label l = Label.Parse( s );
      Assert.That( l.Count, Is.EqualTo( 1 ) );
      Assert.That( l.Single().First(), Is.EqualTo( "A" ) );
      Assert.That( l.Single().Skip( 1 ).First(), Is.EqualTo( "B" ) );
    }

    [Test]
    public void ParseDisjunction() {
      string s = "{{ A }, { B }}";
      Label l = Label.Parse( s );
      Assert.That( l.Count, Is.EqualTo( 2 ) );
      Assert.That( l.First().Single(), Is.EqualTo( "A" ) );
      Assert.That( l.Skip( 1 ).First().Single(), Is.EqualTo( "B" ) );
    }

    [Test]
    public void ParseMix() {
      string s = "{{ A }, { B, C }}";
      Label l = Label.Parse( s );
      Assert.That( l.Count, Is.EqualTo( 2 ) );
      Assert.That( l.First().Single(), Is.EqualTo( "A" ) );
      Assert.That( l.Skip( 1 ).First().First(), Is.EqualTo( "B" ) );
      Assert.That( l.Skip( 1 ).First().Skip( 1 ).First(), Is.EqualTo( "C" ) );
    }

    [Test]
    public void ParseSingleton() {
      string s = "{{ A }}";
      Label l = Label.Parse( s );
      Assert.That( l.Single().Single(), Is.EqualTo( "A" ) );
    }
  }
}