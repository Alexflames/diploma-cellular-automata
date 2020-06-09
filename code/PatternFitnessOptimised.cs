private float CalculateFitnessOptimised(byte[] texturePixels, int texW, int texH, Pattern[] patterns)
{
 float fitness = 0;

 int textureWidth = texW; int textureHeight = texH;
 var tex2D = texW * texH;
 var patternHeight = patterns[0].patternSizeY;
 var patternWidth = patterns[0].patternSizeX;
 var patternErrors = patterns[0].patternErrors;
 var patternRule = patterns[0].pattern;
 int[] curErr = new int[patterns.Length];
 int patternCycle = (1 << patternHeight);

 int[] patternLines = new int[patterns[0].patternSizeX];
 int newPatternLine = 0;
 for (int i = 0; i < patternHeight; i++)
 {
   newPatternLine = 0;
   for (int j = 0; j < patternWidth; j++)
   {
       newPatternLine = (newPatternLine << 1) + patterns[0].pattern[i * patternWidth + j];
   }
   patternLines[i] = newPatternLine;
 }

 int qOffset = 0;

 int newScreenLine = 0;
 for (qOffset = 0; qOffset < patternHeight - 1; qOffset++)
 {
   newScreenLine = 0;
   for (int j = 0; j < patternWidth; j++)
   {
     newScreenLine = (newScreenLine << 1) + texturePixels[qOffset * patternWidth + j];
   }
   screenLines[qOffset] = newScreenLine;
 }

 int newLine = 0, iPix = 0, newPixInd = 0, cornerPixel = 0;
 int screenLine = 0, newErr = 0, nextPixInd = 0;
 for (short i = 0; i < textureHeight; i++)
 {
   newLine = 0;
   iPix = ((i + patternHeight - 1) % textureHeight) * textureWidth;
   for (int j = 0; j < patternWidth; j++)
   {
     newPixInd = (iPix + j);
     newLine = (newLine << 1) + texturePixels[newPixInd];
   }
   qOffset = (qOffset + 1) % patternHeight;
   screenLines[qOffset] = (newLine);

   for (short j = 0; j < textureWidth; j++)
   {
     cornerPixel = j + i * textureWidth;
     for (byte p = 0; p < patterns.Length; p++)
     {
         curErr[p] = 0;
     }
     int minErrors = patternErrors + 1;
     
     for (byte k = 0; k < patternHeight; k++)
     {
       int ind = (qOffset + k) % patternHeight;
       screenLine = screenLines[ind];
       newErr = patternLines[k] ^ screenLine;

       for (byte p = 0; p < patterns.Length; p++)
       {
         newErr = newErr - ((newErr >> 1) & 0x55555555);
         newErr = (newErr & 0x33333333) + ((newErr >> 2) & 0x33333333);
         curErr[p] += (((newErr + (newErr >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
       }

       screenLine <<= 1;
       if (screenLine >= patternCycle) screenLine -= patternCycle;
       nextPixInd = (((i + k) % textureHeight) * textureWidth) + (j + patternWidth) % textureWidth;
       screenLine += texturePixels[nextPixInd];
       
       screenLines[(qOffset + k) % patternHeight] = (screenLine);
     }
     
     for (byte p = 0; p < patterns.Length; p++)
     {
         if (curErr[p] < minErrors) minErrors = curErr[p];
     }
     fitness += minErrors <= patternErrors ? 
      (1 + patternErrors - minErrors) * 1f / (patternErrors + 1) : 0;
   }

 }

 return fitness * patternWidth * patternHeight * 100 / (textureWidth * textureHeight);
}