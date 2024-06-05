using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class KraKpiViewModelCollection : IEnumerable<KraKpiViewModel>
{
    private List<KraKpiViewModel> _viewModels;
    public DateTime? Duration { get; set; }

    public KraKpiViewModelCollection(IEnumerable<KraKpiViewModel> viewModels)
    {
        _viewModels = viewModels.ToList();
    }

    public IEnumerator<KraKpiViewModel> GetEnumerator()
    {
        return _viewModels.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}