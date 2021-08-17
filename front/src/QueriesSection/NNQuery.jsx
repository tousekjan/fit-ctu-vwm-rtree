import React, { useState } from 'react';
import { ContentBase } from '../_common/ContentBase';
import PointFormFields from '../_common/PointFormFields';
import NnQueryResult from '../_common/NnQueryResult';

import * as TreeService from '../_services/TreeService';

import { useTreeInfo } from '../_hooks/index';
import { Form, Button, Spin } from 'antd';

const KNQuery = ({ form }) => {
  const [calling, setCalling] = useState(false);
  const [result, setResult] = useState(null);
  const info = useTreeInfo();

  if (!info)
    return (
      <ContentBase title='Nearest query'>
        <Spin />
      </ContentBase>
    );

  const handleSubmitAsync = async e => {
    e.preventDefault();
    form.validateFields(async (err, values) => {
      if (!err) {
        setResult(null);
        setCalling(true);
        try {
          const result = await TreeService.getNNAsync(values);
          if (result) setResult(result);
        } finally {
          setCalling(false);
        }
      }
    });
  };

  const handleSubmitWithBenchmarkAsync = async e => {
    e.preventDefault();
    form.validateFields(async (err, values) => {
      if (!err) {
        setResult(null);
        setCalling(true);
        try {
          const result = await TreeService.getBenchmarkNNAsync(values);
          if (result) setResult(result);
        } finally {
          setCalling(false);
        }
      }
    });
  };

  return (
    <ContentBase title='Nearest query'>
      <h1>Perform nearest neighbour Query</h1>

      <Form layout='inline' onSubmit={handleSubmitAsync}>
        <PointFormFields
          count={info.dimensions}
          getFieldDecorator={form.getFieldDecorator}
        />
        <br />
        <br />
        <Form.Item>
          <Button
            icon='search'
            disabled={calling}
            type='primary'
            htmlType='submit'
          >
            Perform query
          </Button>
        </Form.Item>
        <Form.Item>
          <Button
            icon='search'
            disabled={calling}
            type='primary'
            onClick={handleSubmitWithBenchmarkAsync}
          >
            Perform query with benchmark
          </Button>
        </Form.Item>
        <Form.Item>
          <Button
            icon='close'
            disabled={calling}
            type='primary'
            onClick={() => setResult(null)}
          >
            Clear result
          </Button>
        </Form.Item>
      </Form>
      {calling && <Spin />}
      {result && (
        <>
          <h2>Result</h2> <NnQueryResult result={result} />
        </>
      )}
    </ContentBase>
  );
};

export default Form.create({ name: 'kn_query' })(KNQuery);
