import React, { useState } from 'react';
import { ContentBase } from '../_common/ContentBase';

import { useViewData, useTreeInfo } from '../_hooks';
import {
  Button,
  notification,
  Spin,
  Modal,
  Input,
  InputNumber,
  Switch,
  Radio
} from 'antd';

import * as SnapshotService from '../_services/SnapshotService';
import * as TreeService from '../_services/TreeService';

const Data = () => {
  const info = useTreeInfo();
  const [dimensions, setDimensions] = useState(2);
  const [pointNumber, setPointNumber] = useState(20);
  const [childrenNumber, setChildrenNumber] = useState(5);
  const [generateRandom, setGenerateRandom] = useState(false);
  const [double, setDouble] = useState(false);
  const [count, setCount] = useState(0);

  const [splitPolicy, setSplitPolicy] = useState('linear');
  const [calling, setCalling] = useState(false);
  const [filename, setfilename] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [showFresh, setShowFresh] = useState(false);
  const [allExpanded, setallExpanded] = useState(false);
  const [view, refreshAsync] = useViewData();

  if (!info)
    return (
      <ContentBase title='View data'>
        <Spin />
      </ContentBase>
    );

  return (
    <ContentBase title='View data'>
      <h1>View Tree data</h1>
      <Button
        type='primary'
        disabled={calling}
        icon='audit'
        onClick={async _ => {
          try {
            setCalling(true);
            await refreshAsync();
            notification['success']({
              message: 'Refresh succes',
              description: 'Data successfully refreshed!'
            });
          } finally {
            setCalling(false);
          }
        }}
      >
        Refresh
      </Button>{' '}
      <Button
        type='primary'
        icon={allExpanded ? 'arrow-up' : 'arrow-down'}
        disabled
        onClick={async _ => {
          setallExpanded(!allExpanded);
        }}
      >
        {' '}
        {allExpanded ? <>Collapse All</> : <>Expand All</>}
      </Button>{' '}
      <Button
        icon='camera'
        type='primary'
        onClick={async _ => {
          setShowModal(true);
        }}
      >
        Take a snapshot
      </Button>{' '}
      <Button
        icon='setting'
        type='dashed'
        onClick={async _ => {
          setShowFresh(true);
        }}
      >
        Replace with fresh
      </Button>
      <br />
      <br />
      {view}
      <Modal
        title='Take a snapshot'
        visible={showModal}
        onOk={async () => {
          try {
            setCalling(true);
            if (await SnapshotService.postSnapShotAsync(filename))
              setShowModal(false);
          } finally {
            setCalling(false);
          }
        }}
        onCancel={() => setShowModal(false)}
        okButtonProps={{ disabled: !filename || filename === '' || calling }}
        cancelButtonProps={{ disabled: calling }}
      >
        <h3> Enter filename (NO extension eg .json)</h3>
        <Input
          onChange={event => {
            setfilename(event.target.value);
          }}
        />
      </Modal>
      <Modal
        title='Replace with fresh tree'
        visible={showFresh}
        onOk={async () => {
          try {
            setCalling(true);
            await TreeService.putFreshAsync({
              dimensions: dimensions,
              maxNumberOfChildren: childrenNumber,
              minNumberOfChildrenRatio: 2,
              maxPointCount: pointNumber,
              double: double,
              count: count,
              splitType: splitPolicy
            });
            await refreshAsync();
            setShowFresh(false);
          } finally {
            setCalling(false);
          }
        }}
        onCancel={() => setShowFresh(false)}
        okButtonProps={{ disabled: calling }}
        cancelButtonProps={{ disabled: calling }}
      >
        <h3> Enter number of dimensions</h3>
        <InputNumber
          onChange={val => setDimensions(val)}
          defaultValue={dimensions}
          min={2}
          placeholder='2'
        />
        <br />
        <h3> Enter max number of children</h3>
        <InputNumber
          onChange={val => setChildrenNumber(val)}
          defaultValue={childrenNumber}
          min={2}
          placeholder='2'
        />
        <br />
        <h3> Enter max number of points in leaf</h3>
        <InputNumber
          onChange={val => setPointNumber(val)}
          defaultValue={pointNumber}
          min={2}
          placeholder='2'
        />
        <br />
        <h3> Split policy</h3>
        <Radio.Group
          value={splitPolicy}
          onChange={e => setSplitPolicy(e.target.value)}
        >
          <Radio.Button value='linear'>Linear</Radio.Button>
          <Radio.Button value='quadratic'>Quadratic</Radio.Button>
          <Radio.Button value='exponential'>Exponential</Radio.Button>
        </Radio.Group>
        <br />
        <br />
        <Switch
          checkedChildren='Generate random'
          unCheckedChildren="Don't generate random"
          defaultChecked={generateRandom}
          onChange={(checked, _) => {
            if (!checked) {
              setCount(0);
            }
            setGenerateRandom(checked);
          }}
        />
        <br />
        {generateRandom && (
          <>
            <br />
            <h3>
              Enter number of points that should be generated and inserted to
              the tree
            </h3>
            <InputNumber
              onChange={val => setCount(val)}
              defaultValue={count}
              min={0}
              placeholder='2'
            />
            <br />
            <br />

            <Switch
              checkedChildren='Double'
              unCheckedChildren='Integer'
              defaultChecked={double}
              onChange={(checked, _) => {
                setDouble(checked);
              }}
            />
          </>
        )}
        {calling && <Spin />}
      </Modal>
    </ContentBase>
  );
};

export default Data;
