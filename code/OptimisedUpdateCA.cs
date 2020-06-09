private void UpdateCA(byte[] CAField, int ind)
{
		long size2D = screenSizeInPixels * screenSizeInPixels;
		int signal = 0;
		
		for (int i = screenSizeInPixels - 2; i < screenSizeInPixels; i++)
		{
				var iPix = i * screenSizeInPixels;
				for (int j = 0; j < screenSizeInPixels; j++)
				{
						screenSignals[iPix + j] = 0;
				}
		}
		for (short i = 0; i < screenSizeInPixels - 2; i++)
		{
				var iPix = i * screenSizeInPixels;
				var iPixm1 = (iPix + size2D - screenSizeInPixels) % size2D;
				var iPixm2 = (iPix + size2D - screenSizeInPixels - screenSizeInPixels) % size2D;
				signal = CAField[iPix + (screenSizeInPixels) - 1] * 2 + CAField[iPix]; 
				for (short j = 1; j < screenSizeInPixels; j++)
				{
						signal = (signal << 1) % 8 + CAField[iPix + j];
						var jm1 = j - 1;
						screenSignals[iPixm2 + jm1] += signal << 6;
						screenSignals[iPixm1 + jm1] += signal << 3;
						screenSignals[iPix   + jm1]  = signal;          
				}
				signal = (signal << 1) % 8 + CAField[iPix];                     
				screenSignals[iPixm2 + screenSizeInPixels - 1] += signal << 6;  
				screenSignals[iPixm1 + screenSizeInPixels - 1] += signal << 3;  
				screenSignals[iPix   + screenSizeInPixels - 1]  = signal;       
		}

		for (int i = screenSizeInPixels - 2; i < screenSizeInPixels; i++)
		{
				var iPix = i * screenSizeInPixels;
				var iPixm1 = (iPix + size2D - screenSizeInPixels) % size2D;
				var iPixm2 = (iPix + size2D - screenSizeInPixels - screenSizeInPixels) % size2D;
				signal = CAField[iPix + (screenSizeInPixels) - 1] * 2 + CAField[iPix];
				for (short j = 1; j < screenSizeInPixels; j++)
				{
						signal = (signal << 1) % 8 + CAField[iPix + j];
						var jm1 = j - 1;
						screenSignals[iPixm2 + jm1] += signal << 6;
						screenSignals[iPixm1 + jm1] += signal << 3;
						screenSignals[iPix + jm1]   += signal;          
				}
				signal = (signal << 1) % 8 + CAField[iPix];                    
				screenSignals[iPixm2 + screenSizeInPixels - 1] += signal << 6;
				screenSignals[iPixm1 + screenSizeInPixels - 1] += signal << 3;
				screenSignals[iPix + screenSizeInPixels - 1] = signal; 
		}

		for (int i = 0; i < size2D; i++)
		{
				CAField[i] = (byte)allRules[ind][screenSignals[(i - screenSizeInPixels + size2D) % size2D]];
		}
}
