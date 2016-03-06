using System.Collections.Generic;

using M8.TruthTeller.Nodes;

using NUnit.Framework;

namespace M8.TruthTeller.UnitTest {
  [TestFixture]
  public class JustificationTests : TestBase {

    [Test]
    /*
     {{A}}
     -------
     |  A  |--\      {{A},{B}}      {{A},{B}}
     -------   \-->  -------        -------
     {{B}}           |  r  |----->  |  g  | --\
     -------   /-->  -------        -------    \        {{A,D},{B,D}}
     |  B  |--/       {{D}}                     \       -------
     -------         -------                     ---->  |  h  |
     {{D}}      -->  |  l  |                     /      -------
     -------   /     -------\        {{D}}      /
     |  D  |--/              \      -------    /
     -------  \      {{D},{E}} ---> |  i  | --/
     {{E}}     \-->  ------- /      -------
     -------         |  k  |/
     |  E  |------>  -------
     -------         
     */ public void IntegratedExample() {
      SetupABr();
      Setupg();
      SetupD();
      SetupE();
      Setupl();
      Setupk();
      Setupi();
      Setuph();

      AssertLabel( "h", "{ {A,D}, {B,D} }" );
    }

    [Test]
    public void Retraction() {
      SetupABr();
      Setupg();
      SetupD();
      SetupE();
      Setupl();
      Setupk();
      Setupi();
      Setuph();

      var justifications = Informant.GetJustificationFor( "r" );
      Assert.That( justifications.Count, Is.EqualTo( 2 ) );

      Recorder.Retract( "A" );
      AssertLabel( "h", "{{B,D}}" );

      justifications = Informant.GetJustificationFor( "r" );
      Assert.That( justifications.Count, Is.EqualTo( 1 ) );

      Recorder.Retract( "E" );
      AssertLabel( "h", "{{B,D}}" );
    }

    [Test]
    /*
                      {{D}}                
                     -------               
     {{D}}      -->  |  l  |               
     -------   /     -------\        {{D}} 
     |  D  |--/              \      -------
     -------  \      {{D},{E}} ---> |  i  |
     {{E}}     \-->  ------- /      -------
     -------         |  k  |/
     |  E  |------>  -------
     -------         
     */ public void MultipleConjunctions() {
      SetupD();
      Setupl();
      SetupE();
      Setupk();
      Setupi();

      AssertLabel( "i", "{{D}}" );
    }

    [Test]
    [Description( "Singleton justifications have the exact same label as their ANode" )]
    /*
     {{D}}        {{D}} 
     -------     -------
     |  D  |-->  |  l  |
     -------     ------ 
     */ public void Singleton() {
      SetupD();
      Setupl();
      AssertLabel( "l", "{ {D} }" );
    }

    [Test]
    [Description( "Singleton justifications have the exact same label as their ANode" )]
    /*
     {{A},{B}}      {{A},{B}}
     -------        -------
     |  r  |----->  |  g  |
     -------        -------
     */ public void SingletonWithDisjointSets() {
      SetupABr();
      Setupg();
      AssertLabel( "g", "{{A},{B}}" );
    }

    [Test]
    [Description( "Conjunction of two ANode's in a single justification" )]
    /*
 {{A}}
 -------
 |  A  |--\      {{A,B}}
 -------   \     -------
 {{B}}      -->  |  r  |
 -------   /     -------
 |  B  |--/
 -------
 */ public void TestConjunction() {
      SetupABrConjoint();
      AssertLabel( "r", "{{A,B}}" );
    }

    [Test]
    [Description( "Two disjoint a nodes in separate justifications" )]
    /*
 {{A}}
 -------
 |  A  |--\      {{A},{B}}
 -------   \-->  -------
 {{B}}           |  r  |
 -------   /-->  -------
 |  B  |--/
 -------
 */ public void TestDisjunction() {
      SetupABr();
      AssertLabel( "r", "{{A},{B}}" );
    }

    [Test]
    [Description( "Conjunction of two ANode's in a single justification" )]
    /*
 {{A}}
 -------
 |  A  |--\      {{A,B}}
 -------   \     -------
 {{B}}      -->  |  r  |
 -------   /     -------
 |  B  |--/
 -------
 */ public void TestLazyConjunction() {
      SetupABr();
      List<Justification> justifications = Informant.GetJustificationFor( "r" );
      Recorder.Conjoin( justifications.ToArray() );
      AssertLabel( "r", "{{A,B}}" );
    }

    [Test]
    public void WhenRecording_ThenInformantKnows() {
      Recorder.Justify( new Node { Id = "Z" } );
      Node node = Informant.GetNode( "Z" );
      Assert.That( node.Id, Is.EqualTo( "Z" ) );
    }
  }
}