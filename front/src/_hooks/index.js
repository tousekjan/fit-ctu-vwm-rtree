import React from 'react';

import DataView from '../_common/DataView';
import DataVisualize from '../_common/DataVisualize';
import { useState, useEffect } from 'react';
import * as TreeService from '../_services/TreeService';
import * as SnapshotService from '../_services/SnapshotService';

export const useTreeInfo = () => {
  const [info, setinfo] = useState(null);

  const fetchInfoAsync = async () => {
    const response = await TreeService.getInfoAsync();

    if (response) setinfo(response);
  };

  useEffect(() => {
    fetchInfoAsync();
  }, []);

  return info;
};

export const useSnapshotPreviews = () => {
  const [data, setData] = useState(null);

  const fetchDataAsync = async () => {
    const response = await SnapshotService.getPreviewsAsync();

    if (response) setData(response);
  };

  useEffect(() => {
    fetchDataAsync();
  }, []);

  return [data, async () => await fetchDataAsync()];
};

export const useTreeData = () => {
  const [data, setData] = useState(null);

  const fetchDataAsync = async () => {
    const response = await TreeService.getDataAsync();

    if (response) setData(response);
  };

  useEffect(() => {
    fetchDataAsync();
  }, []);

  return [data, async () => await fetchDataAsync()];
};

export const useViewData = () => {
  const [data, refreshAsync] = useTreeData();

  if (!data) return [<div />, refreshAsync];

  return [<DataView data={data} />, refreshAsync];
};

export const useVisualizeData = info => {
  const [data, refreshAsync] = useTreeData();

  if (!data) return [<div />, refreshAsync];

  return [<DataVisualize data={data} info={info} />, refreshAsync];
};

export const useTheme = () => {
  const [theme, setTheme] = useState(localStorage.getItem('theme'));

  const set = newTheme => {
    setTheme(newTheme);
    newTheme && localStorage.setItem('theme', newTheme);
  };

  return [theme, set];
};
