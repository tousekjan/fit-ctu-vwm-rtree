import React from 'react';
import { Menu, Icon } from 'antd';
import { withRouter } from 'react-router-dom';
import * as Routes from '../Routes';

const TopBar = ({ history, location, theme }) => (
  <div style={{ position: 'fixed', zIndex: 1, width: '100%' }}>
    <Menu
      onClick={e => history.push(e.key)}
      selectedKeys={[location.pathname]}
      mode='horizontal'
      theme={theme}
    >
      <Menu.Item key={Routes._Home}>
        <Icon type='home' />
        Home
      </Menu.Item>

      <Menu.Item key={Routes._AddPoint}>
        <Icon type='upload' />
        Add Point
      </Menu.Item>

      <Menu.Item key={Routes._RangeQuery}>
        <Icon type='global' />
        Range Query
      </Menu.Item>

      <Menu.Item key={Routes._NNQuery}>
        <Icon type='tag' />
        NN Query
      </Menu.Item>

      <Menu.Item key={Routes._KNNQuery}>
        <Icon type='filter' />
        KNN Query
      </Menu.Item>

      <Menu.Item key={Routes._ContainsQuery}>
        <Icon type='file-search' />
        Contains
      </Menu.Item>

      <Menu.Item key={Routes._Data}>
        <Icon type='database' />
        Data
      </Menu.Item>

      <Menu.Item key={Routes._Visualize}>
        <Icon type='eye' />
        Visualize
      </Menu.Item>

      <Menu.Item key={Routes._Snapshots}>
        <Icon type='camera' />
        Snapshots
      </Menu.Item>
    </Menu>
  </div>
);

export default withRouter(TopBar);
