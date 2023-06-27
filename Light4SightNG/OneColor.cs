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
                        return baseIntensity * (1 - ratios[activeLED]);
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
                ratios[activeLED] = ratios[activeLED] + 0.025;
            }

            public void decrementRatio()
            {
                ratios[activeLED] = ratios[activeLED] - 0.025;
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
                    switch(value)
                    {
                    case "Red":
                        activeLED = 0;
                        break;
                    case "Green":
                        activeLED = 1;
                        break;
                    case "Blue":
                        activeLED = 2;
                        break;
                    case "Cyan":
                        activeLED = 3;
                        break;
                    default:
                        activeLED = 0;
                        break;
                    }

                }
            }
        }

}
