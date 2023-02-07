namespace CalibrateLEDStimulator
{
        class ColorMatch
        {

            string[] ratioNames = { "redVScyan", "greenVSblue", "luminance" };
            double[] ratios;

            public ColorMatch()
            {
                ratios = new double[] { 0.0, 0.0, 0.0 };
            }

            public ColorMatch(double[] initRatios)
            {
                ratios = initRatios;
            }

            public void incrementRed()
            {
                ratios[0] += 0.025;
            }

            public void incrementCyan()
            {
                ratios[0] -= 0.025;
            }

            public void incrementGreen()
            {
                ratios[1] += 0.025;
            }

            public void incrementBlue()
            {
                ratios[1] -= 0.025;
            }

            public void incrementRedCyan()
            {
                ratios[2] += 0.025;
            }

            public void incrementGreenBlue()
            {
                ratios[2] -= 0.025;
            }

            public double[] SettingsLED
            {
                get
                {
                    double[] leds = new double[8];

                    // set Red-Cyan-Ratio (outer)
                    leds[0] = 1.0;
                    leds[1] = 0.0;
                    leds[2] = 0.0;
                    leds[3] = 1.0 + ratios[0];
                    // set Green-Blue-Ratio (inner)
                    leds[4] = 0.0;
                    leds[5] = 1.0;
                    leds[6] = 1.0 + ratios[1];
                    leds[7] = 0.0;
                    //set Luminance ratio
                    leds[0] = leds[0] * (1 + ratios[2]);
                    leds[3] = leds[3] * (1 + ratios[2]);

                    double maxIntensity = 0.0;
                    for (int i = 0; i < 8; i++)
                    {
                        if (leds[i] > maxIntensity) maxIntensity = leds[i];
                    }
                    for (int i = 0; i < 8; i++)
                    {
                        leds[i] = leds[i] / maxIntensity;
                    }

                    return leds;
                }
            }
            
            public double RatioRedCyan
            {
                get { return ratios[0]; }
            }

            public double RatioGreenBlue
            {
                get { return ratios[1]; }
            }

            public double RatioLuminance
            {
                get { return ratios[2]; }
            }

        }

}
