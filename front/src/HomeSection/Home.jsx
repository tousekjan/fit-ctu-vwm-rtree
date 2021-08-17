import React from 'react';
import { ContentBase } from '../_common/ContentBase';
import { useTreeInfo } from '../_hooks';
import { Spin } from 'antd';

const Home = () => {
  const info = useTreeInfo();

  if (!info)
    return (
      <ContentBase title='Home'>
        <Spin />
      </ContentBase>
    );

  return (
    <ContentBase title='Home'>
      <h1>Home</h1>
      <p>
        Seminar paper RTree implementation for <b>BI-VWM</b> <b>FIT CTU</b>.
      </p>
      <h2>F&Q</h2>
      <ul>
        <li>
          <i>What is number of dimensions in this RTree implemenation ? </i>{' '}
          <b>Currently it is set to {info.dimensions}</b>
        </li>
        <br/>
        <li>
          <i>What kind of split algorith was used to build current tree ? </i>{' '}
          <b>{info.splitType} split</b>
        </li>
        <br/>
         <li>
          <i>What is max number of children in one inner node in current tree ? </i>{' '}
          <b>{info.maxNumberOfChildren}</b>
        </li>
        <br/>
        <li>
          <i>What is max number of points in one leaf node in current tree ? </i>{' '}
          <b>{info.maxPointCount}</b>
        </li>
        <br/>
        <li>
          <i>What is max count of points returned in KN query ? </i>{' '}
          <b>{info.maxKNQuerySize || '200'}</b>
        </li>
        <br/>
        <li>
          <i>Where can i take snapshot of the Tree ? </i>{' '}
          <b>In <a href="/#/data">data</a> section</b>
        </li>
        <br/>
        <li>
          <i>Where can i create fresh instance of the Tree ? </i>{' '}
          <b>In <a href="/#/data">data</a> section</b>
        </li>
      </ul>
    </ContentBase>
  );
};

export default Home;
