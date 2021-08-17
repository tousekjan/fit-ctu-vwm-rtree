import React from 'react';

import { Form, InputNumber } from 'antd';

const PointFormFields = ({ count, getFieldDecorator }) => {
  const fields = [];

  for (let i = 1; i <= count; i++) {
    fields.push(
      <Form.Item key={i} label={i}>
        {getFieldDecorator(i.toString(), {
          initialValue: 0,
          rules: [{ required: true, message: 'Field is required!' }]
        })(<InputNumber placeholder='0' />)}
      </Form.Item>
    );
  }

  return <>{fields}</>;
};

export default PointFormFields;
