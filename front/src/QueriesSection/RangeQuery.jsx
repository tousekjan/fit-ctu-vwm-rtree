import React, { useState } from 'react';
import { ContentBase } from '../_common/ContentBase';
import PointFormFields from '../_common/PointFormFields';
import QueryResult from '../_common/QueryResult';

import * as TreeService from '../_services/TreeService';

import { useTreeInfo } from '../_hooks/index';
import { Form, Button, InputNumber, Spin } from 'antd';

const RangeQuery = ({ form }) => {
  const [calling, setCalling] = useState(false);
  const [result, setResult] = useState(null);
  const info = useTreeInfo();

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
        setResult(null);
        setCalling(true);
        try {
          var res = await TreeService.getRangeAsync(values);
          setResult(res);
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
          var res = await TreeService.getBenchmarkRangeAsync(values);
          setResult(res);
        } finally {
          setCalling(false);
        }
      }
    });
  };

  return (
    <ContentBase title='Range query'>
      <h1>Perform Range Query</h1>

      <Form layout='inline' onSubmit={handleSubmitAsync}>
        <PointFormFields
          count={info.dimensions}
          getFieldDecorator={form.getFieldDecorator}
        />
        <br />
        <Form.Item key='Range' label='Range'>
          {form.getFieldDecorator('Range', {
            initialValue: 10,
            rules: [{ required: true, message: 'Field is required!' }]
          })(<InputNumber min={1} placeholder='0' />)}
        </Form.Item>
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
      <br />
      {calling && <Spin />}
      {result && (
        <>
          <h2>Result</h2> <QueryResult result={result} />
        </>
      )}
    </ContentBase>
  );
};

export default Form.create({ name: 'range_query' })(RangeQuery);
