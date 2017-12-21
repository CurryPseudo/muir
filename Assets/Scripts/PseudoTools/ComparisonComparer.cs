using System.Collections;
using System.Collections.Generic;
using System;
public class ComparisonComparer<T> : IComparer<T>
{
    private readonly Comparison<T> comparison;

    public ComparisonComparer(Comparison<T> comparison)
    {
        this.comparison = comparison;
    }

    int IComparer<T>.Compare(T x, T y)
    {
        return comparison(x, y);
    }
}