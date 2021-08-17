import React, { useState } from 'react';
import { ContentBase } from '../_common/ContentBase';
import { useTreeInfo, useVisualizeData } from '../_hooks';
import { Button, notification, Spin } from 'antd';

const Visualize = () => {
  const [calling, setCalling] = useState(false);
  const info = useTreeInfo();
  const [visualize, refreshAsync] = useVisualizeData(info);

  if (!info)
    return (
      <ContentBase title='Visualize'>
        <Spin />{' '}
      </ContentBase>
    );

  if (info.dimensions > 2)
    return (
      <ContentBase title='Visualize'>
        <h1>Visualize</h1>
        {visualize}
      </ContentBase>
    );

  return (
    <ContentBase title='Visualize'>
      <h1>Visualize</h1>
      <Button
        icon='audit'
        type='primary'
        disabled={calling}
        onClick={async _ => {
          setCalling(true);
          try {
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
      </Button>
      <br />
      <br />
      {visualize}
    </ContentBase>
  );
};

export default Visualize;
