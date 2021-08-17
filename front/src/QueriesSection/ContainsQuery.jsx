import React, { useState } from 'react';
import { ContentBase } from '../_common/ContentBase';
import PointFormFields from '../_common/PointFormFields';

import * as TreeService from '../_services/TreeService';

import { useTreeInfo } from '../_hooks/index';
import { Form, Button, Spin } from 'antd';

const writePoint = point =>
  point.coords.reduce(
    (previous, current) =>
      previous === '(' ? previous + current : previous + `, ${current}`,
    '('
  ) + ')';

const ContainsQuery = ({ form }) => {
  const [calling, setCalling] = useState(false);
  const [result, setResult] = useState(null);
  const info = useTreeInfo();

  if (!info)
    return (
      <ContentBase title='Contains Query'>
        <Spin />
      </ContentBase>
    );

  const handleSubmitAsync = async e => {
    e.preventDefault();
    form.validateFields(async (err, values) => {
      if (!err) {
        setCalling(true);
        try {
          var result = await TreeService.containsAsync(values);
          if (result) setResult(result);
        } finally {
          setCalling(false);
        }
      }
    });
  };

  return (
    <ContentBase title='Contains Query'>
      <h1>Perform Contains query</h1>

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
      {result != null && (
        <>
          <h2>Result</h2>
          {result.contains
            ? 'RTree contains point ' + writePoint(result.requestPoint)
            : 'RTree does not contain point' + writePoint(result.requestPoint)}
          . <i>Performed in {result.treeMilliseconds} ms.</i>
        </>
      )}
    </ContentBase>
  );
};

export default Form.create({ name: 'kn_query' })(ContainsQuery);
