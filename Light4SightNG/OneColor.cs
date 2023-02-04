namespace CalibrateLEDStimulator
{
        class OneColor
        {
            double[] ratios;
            int activeLED = 0;
            double baseIntensity = 0.5;

            public OneColor()
            {
                ratios = new double[] { 0.0, 0.0, 0.0, 0.0 };
            }

            public OneColor(double[] initRatios)
            {
                ratios = initRatios;
            }

            public double IntensityInner
            {
                get
                {
                    if (ratios[activeLED] > 0)
                    {
                        return baseIntensity / (1 + ratios[activeLED]);
                    }
                    else
                    {
                        return baseIntensity;
                    }
                }
            }

            public double IntensityOuter
            {
                get
                {
                    if (ratios[activeLED] > 0)
                    {
                        return baseIntensity;
                    }
                    else
                    {
                        return baseIntensity * (1 + ratios[activeLED]);
                    }
                }
            }

            public double BaseIntensity
            {
                set
                {
                    if (value < 0) baseIntensity = 0;
                    else if (value > 1) baseIntensity = 1;
                    else baseIntensity = value;
                }
            }

            public void NextLED()
            {
                activeLED += 1;
                if (activeLED == 4) activeLED = 0;
            }

            public void PreviousLED()
            {
                activeLED -= 1;
                if (activeLED == -1) activeLED = 3;
            }

            public int ActiveLED
            {
                get { return activeLED; }
            }

            public void incrementRatio()
            {
                ratios[activeLED] += 0.25;
            }

            public void decrementRatio()
            {
                ratios[activeLED] -= 0.25;
            }

            public double RatioRED
            {
                get { return ratios[0]; }
            }

            public double RatioGREEN
            {
                get { return ratios[1]; }
            }

            public double RatioBLUE
            {
                get { return ratios[2]; }
            }

            public double RatioCYAN
            {
                get { return ratios[3]; }
            }

            public string SetActiveLED
            {
                set
                {
                    activeLED = value switch
                    {
                        "Red" => 0,
                        "Green" => 1,
                        "Blue" => 2,
                        "Cyan" => 3,
                        _ => 0,
                    };
                }
            }
        }

}
