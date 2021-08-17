import React, { useState } from 'react';
import { ContentBase } from '../_common/ContentBase';
import PointFormFields from '../_common/PointFormFields';

import * as TreeService from '../_services/TreeService';
import { useTreeInfo, useViewData, useVisualizeData } from '../_hooks';
import { Form, Button, Spin } from 'antd';

const AddPoint = ({ form }) => {
  const [calling, setCalling] = useState(false);
  const [hideVisual, setHideVisual] = useState(
    localStorage.getItem('viusalizationHiden') === 'true'
  );
  const [view, refreshAsync] = useViewData();
  const info = useTreeInfo();
  const [visualize, refreshVisualAsync] = useVisualizeData(info);

  if (!info)
    return (
      <ContentBase title='Add Point'>
        <Spin />
      </ContentBase>
    );

  const handleSubmitAsync = async e => {
    e.preventDefault();
    form.validateFields(async (err, values) => {
      if (!err) {
        setCalling(true);
        try {
          await TreeService.postPointAsync(values);
          await refreshAsync();
          await refreshVisualAsync();
        } finally {
          setCalling(false);
        }
      }
    });
  };

  return (
    <ContentBase title='Add Point'>
      <h1>Add point to RTree</h1>
      <Form layout='inline' onSubmit={handleSubmitAsync}>
        <PointFormFields
          count={info.dimensions}
          getFieldDecorator={form.getFieldDecorator}
        />
        <br />
        <br />
        <Form.Item>
          <Button
            icon='upload'
            disabled={calling}
            type='primary'
            htmlType='submit'
          >
            Add Point
          </Button>
        </Form.Item>{' '}
        <Form.Item>
          <Button
            icon={
              localStorage.getItem('viusalizationHiden') === 'true'
                ? 'eye'
                : 'eye-invisible'
            }
            disabled={calling}
            type='primary'
            onClick={_ => {
              if (localStorage.getItem('viusalizationHiden') === 'true') {
                localStorage.removeItem('viusalizationHiden');
                setHideVisual(false);
              } else {
                localStorage.setItem('viusalizationHiden', 'true');
                setHideVisual(true);
              }
            }}
          >
            {hideVisual ? 'Show visualization' : 'Hide visualization'}
          </Button>
        </Form.Item>
      </Form>
      <br />
      {!hideVisual && (
        <>
          <h2>Tree data Visualized</h2>
          {visualize}
        </>
      )}
      <h2>Tree data Raw</h2>
      {view}
    </ContentBase>
  );
};

export default Form.create({ name: 'add_point' })(AddPoint);
