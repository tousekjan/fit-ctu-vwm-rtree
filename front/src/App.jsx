import React from 'react';
import { Route, HashRouter, Redirect } from 'react-router-dom';
import * as Routes from './Routes';
import { Layout, Switch } from 'antd';

import Data from './InfoSection/Data';
import Home from './HomeSection/Home';
import TopBar from './Topbar/Topbar';
import RangeQuery from './QueriesSection/RangeQuery';
import KNNQuery from './QueriesSection/KNNQuery';
import NNQuery from './QueriesSection/NNQuery';
import AddPoint from './AddPointSection/AddPoint';
import Visualize from './VisualizeSection/Visualize';
import Contains from './QueriesSection/ContainsQuery';
import Snapshots from './SnapshotSection/Snapshots';
import { useTheme } from './_hooks';

const { Footer } = Layout;

const HomeRedirect = () => <Redirect to={Routes._Home} />;

const App = () => {
  const [theme, setTheme] = useTheme();

  return (
    <HashRouter>
      <Layout>
        <TopBar theme={theme} />

        <Route exact path='/' component={HomeRedirect} />
        <Route exact path={Routes._Home} component={Home} />
        <Route exact path={Routes._AddPoint} component={AddPoint} />
        <Route exact path={Routes._RangeQuery} component={RangeQuery} />
        <Route exact path={Routes._KNNQuery} component={KNNQuery} />
        <Route exact path={Routes._NNQuery} component={NNQuery} />
        <Route exact path={Routes._ContainsQuery} component={Contains} />
        <Route exact path={Routes._Data} component={Data} />
        <Route exact path={Routes._Visualize} component={Visualize} />
        <Route exact path={Routes._Snapshots} component={Snapshots} />
        <Footer
          style={{
            textAlign: 'center'
          }}
        >
          RTree ©2019 prixladi, touseja4{' '}
          <Switch
            checkedChildren='dark'
            unCheckedChildren='light'
            defaultChecked={theme === 'dark'}
            onChange={(checked, _) => {
              const newTheme = checked ? 'dark' : 'light';
              setTheme(newTheme);
            }}
          />
        </Footer>
      </Layout>
    </HashRouter>
  );
};

export default App;
