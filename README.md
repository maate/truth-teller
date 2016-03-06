Truth-teller is a .NET implementation of an assumption based truth maintenance system (ATMS).

It follows the implementation described by Jon Doyle in "A Truth Maintenance System" in Artificial Intelligence Vol. 12. No 3, pp. 251–272. 1979.

This code is not widely used and probably contains errors. Let me know if you encounter any of these.

## Usage

Quick example. We create a TMS with two nodes A and B.

Each node independently justifies a third node, r:

    var tms = new TMS();
    Recorder recorder = new TruthRecorder( tms );
    
    recorder.Assume( new Node { Id = "A" } );
    recorder.Assume( new Node { Id = "B" } );
    recorder.Justify( new Node { Id = "r" } ).WithAntecedents( "A" );
    recorder.Justify( new Node { Id = "r" } ).WithAntecedents( "B" );

This results in the following TMS:
    
     {{A}}
     -------
     |  A  |--\      {{A},{B}}
     -------   \-->  -------
     {{B}}           |  r  |
     -------   /-->  -------
     |  B  |--/      
     -------         

Read out information by using a `TruthTeller`:

    TruthTeller informant = new TruthTeller( tms );
    var node = Informant.GetNode( "r" );
    Console.WriteLine( node.Label );
    
     
## License
The MIT License (MIT)
Copyright (c) 2016 Morten Maate

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

