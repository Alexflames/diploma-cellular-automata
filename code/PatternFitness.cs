private float CalculateFitness(byte[] texturePixels, int texW, int texH, Pattern[] patterns)
{
	float fitness = 0;

	int textureWidth = texW; int textureHeight = texH;

	var patternHeight = patterns[0].patternSizeY;
	var patternWidth = patterns[0].patternSizeX;
	var patternErrors = patterns[0].patternErrors;
	var patternsCount = patterns.Length;
	int[] currentErrors = new int[patternsCount];

	for (short i = 0; i < textureHeight; i++)
	{
		for (short j = 0; j < textureWidth; j++)
		{
			int cornerPixel = j + i * textureWidth;
			for (int p = 0; p < patternsCount; p++)
			{
				currentErrors[p] = 0;
			}

			for (byte ir = 0; ir < patternHeight; ir++)
			{
				for (byte jr = 0; jr < patternWidth; jr++)
				{
					int pixelIndex = (cornerPixel + ir * textureWidth + jr) 
						% (textureWidth * textureHeight);
					for (int p = 0; p < patternsCount; p++)
					{
						currentErrors[p] += texturePixels[pixelIndex] == 
							patterns[p].pattern[ir * patternWidth + jr] ? 0 : 1;
					}
				}
			}
			int minErrors = patternErrors + 1;
			for (int p = 0; p < patternsCount; p++)
			{
				minErrors = currentErrors[p] < minErrors ? currentErrors[p] : minErrors;
			}

			fitness += minErrors <= patternErrors ? 
				(1 + patternErrors - minErrors) * 1f / (patternErrors + 1) : 0;
		}
	}
	return fitness * 100 / (textureWidth * textureHeight);
}