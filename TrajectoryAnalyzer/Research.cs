using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using MathNet.Numerics.IntegralTransforms;
using System.Numerics;

namespace TrajectoryAnalyzer
{
    public class Research
    {
        public Research(List<double> propertyValues)
        {
            propertyValues_ = propertyValues;
            Mean = propertyValues_.Average();
            Variance = propertyValues_.Select(x => Math.Pow(x - Mean, 2)).Average();
            Std = Math.Sqrt(Variance);
            //AutocorrelationWitFFT();
            //FFT();
        }

        private void AutocorrelationWitFFT()
        {
            int n = propertyValues_.Count;
            Complex[] samples = new Complex[2 * n];
            for (int i = 0; i < n; i++)
            {
                samples[i] = new Complex(propertyValues_[i] - Mean, 0);
            }
            for (int i = n; i < 2 * n; i++)
            {
                samples[i] = new Complex(0, 0);
            }

            Fourier.Forward(samples, FourierOptions.Matlab);
            for (int t = 0; t < propertyValues_.Count; t++)
            {
                samples[t] = samples[t] * Complex.Conjugate(samples[t]);
            }
            Fourier.Inverse(samples, FourierOptions.Matlab);

            act_ = new List<double>(n);
            for (int t = 0; t < n; t++)
            {
                act_.Add(samples[t].Real);
            }
            double max = act_.Max();
            for (int t = 0; t < n; t++)
            {
                act_[t] /= max;
            }
        }

        private void FFT()
        {
            var act = Act;
            Complex[] samples = new Complex[act.Count];
            for (int i = 0; i < act.Count; i++)
            {
                samples[i] = new Complex(act[i], 0);
            }
            Fourier.Forward(samples, FourierOptions.Matlab);
            fftAct_ = new List<double>(act.Count / 2);
            for (int i = 0; i < act.Count / 2; i++)
            {
                fftAct_.Add(samples[i].Real);
            }
        }

        public List<double> GetData()
        {
            return propertyValues_;
        }

        public Dictionary<double, double> GetHisData(int stepsNumber = 25)
        {
            double step = (propertyValues_.Max() - propertyValues_.Min()) / stepsNumber;

            var bucketeer = new Dictionary<double, double>();
            for (double curr = propertyValues_.Min(); curr <= propertyValues_.Max(); curr += step)
            {
                // Counting the values that can be put in the bucket and dividing them on values.Count()
                double count = propertyValues_.Where(x => x >= curr && x < curr + step).Count();
                bucketeer.Add(curr + step / 2, count / propertyValues_.Count() / step);
            }

            return bucketeer;
        }

        public Dictionary<double, double> GetNormalDistribution(int stepsNumber = 25)
        {
            var normalDistribution = new Dictionary<double, double>();

            double x0 = propertyValues_.Min();
            double x1 = propertyValues_.Max();
            for (int i = 0; i < stepsNumber; i++)
            {
                double x = x0 + (x1 - x0) * i / (stepsNumber - 1);
                double f = 1.0 / Math.Sqrt(2 * Math.PI * Variance) * Math.Exp(-(x - Mean) * (x - Mean) / 2 / Variance);
                normalDistribution.Add(x, f);
            }
            return normalDistribution;
        }

        public List<double> PropertyValues
        {
            get
            {
                Debug.Assert(propertyValues_ != null);
                return propertyValues_;
            }
        }

        public List<double> Act
        {
            get
            {
                if (act_ == null)
                {
                    AutocorrelationWitFFT();
                }
                return act_;
            }
        }

        public List<double> FftAct
        {
            get
            {
                if (fftAct_ == null)
                {
                    FFT();
                }
                return fftAct_;
            }
        }

        public double Mean
        {
            get;
        }
        public double Variance
        {
            get;
        }
        //
        // Summary:
        //     Gets a Standard Deviation of Property values.
        //
        // Returns:
        //     Standard Deviation.
        public double Std
        {
            get;
        }

        private List<double> propertyValues_;
        private List<double> act_;
        private List<double> fftAct_;
    }
}
