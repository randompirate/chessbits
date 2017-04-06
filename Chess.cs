using System;
using System.Diagnostics;

public class Chess
{
	ulong[] P = ChessPieces.P;
	ulong[] k = ChessPieces.k;
	ulong[] B = ChessPieces.B;
	ulong[] R = ChessPieces.R;
	ulong[] Q = ChessPieces.Q;
	ulong[] K = ChessPieces.K;
	
	//Magic numbers:
	const ulong octant  = 135007759UL; 
	const ulong tophalf = (1UL<<32)-1;
	const ulong bothalf = ~tophalf;
	const ulong rghhalf = 1085102592571150095;
	const ulong lfthalf = ~rghhalf;
	const ulong botleft = 9169081515752227072;
	
	
	int calls = 0;
	//int[] places;
	
	Chess(){}
	
	public ulong[][] stringParse(string s)
	{
		ulong[][] ret = new ulong[s.Length][];
		for(int i=0; i<s.Length; i++)
		{
			switch(s.Substring(i,1))
			{
				case "P": ret[i] = P; break;
				case "k": ret[i] = k; break;
				case "B": ret[i] = B; break;
				case "R": ret[i] = R; break;
				case "Q": ret[i] = Q; break;
				case "K": ret[i] = K; break;
			}
		}
		// Todo: sort by strength
		return ret;
	}
		
	public static string bingrid(ulong x)
	{
		string ret = " +--------+\n |";
		for(int i=0; i<64; i++)
		{
			ret += ((x&(1UL<<i))>0)?'#':' ';
			if(i%8==7) ret +="|\n |";
		}
		ret += "\b+--------+";
		return ret;
	}
	
	public static void printSolution()
	{
	
	}
	
	public bool Solve(ulong[][] pieceArray)
	{
		return this.Solve(pieceArray, 0, 0UL, 0UL);
	}
	
	public bool Solve(ulong[][] pieceArray, int nextPiece, ulong controlledCells, ulong placements)
	{
		this.calls++;
		if(pieceArray.Length == nextPiece)
		{
			Console.WriteLine("Solution: ");
			Console.WriteLine(bingrid(placements));
			return true;
		}
		
		//Pick the next piece
		ulong[] piece = pieceArray[nextPiece];
		//Try all cells
		for(int cell = 0; cell<64; cell++)
		{
			// Todo: Test symmetry on controlled cells
			// Exploit symmetry of empty board (useful if no solution is possible)
			if(nextPiece == 0)
			{
				// Skip everything not in the first octant
				while((((1UL<<cell)&octant) ==0) && cell<64) cell++;
			}
			
			//Test if it is free
			if((1UL<<cell&controlledCells) == 0)
			{
				//Test if the new piece attacks any placed pieces
				if((piece[cell]&placements) == 0)
				{
					//Place it and solve recursively
					if(Solve(
							pieceArray,
							nextPiece + 1,
							controlledCells	|piece[cell],
							placements	|(1UL<<cell)
							))
					{		
						//TODO: Add to placements
						
						return true;
					}
				}
			}
		}
		
		return false;
	}

	public static void Main(string[] args)
	{
		if(args.Length == 0) args=new string[]{"QQQQQQQQ"};
	
		Chess c = new Chess();
	
		Stopwatch sw = new Stopwatch();
		Console.WriteLine("Solve for the set: " + args[0]);
		sw.Start();
		if(!c.Solve(c.stringParse(args[0]))) Console.WriteLine("No solution");
		Console.WriteLine("Time taken: " + sw.ElapsedMilliseconds + " ms");
		Console.WriteLine("Visited nodes: " + c.calls);
	
	}

}
