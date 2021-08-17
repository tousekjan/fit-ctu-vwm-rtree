import React, { useState } from 'react';
import { ContentBase } from '../_common/ContentBase';
import { useSnapshotPreviews } from '../_hooks';
import { Spin, List, Card, Button, Empty, Popconfirm } from 'antd';
import * as SnapshotService from '../_services/SnapshotService';

const Home = () => {
  const [previews, refresh] = useSnapshotPreviews();
  const [calling, setCalling] = useState(false);

  if (!previews)
    return (
      <ContentBase title='Snapshots'>
        <Spin />
      </ContentBase>
    );

  return (
    <ContentBase title='Snapshots'>
      <h1>Snapshots</h1>
      <List
        locale={{ emptyText: <Empty /> }}
        grid={{
          gutter: 25,
          column: 3
        }}
        dataSource={previews}
        renderItem={preview => (
          <List.Item>
            <Card title={preview.name}>
              <i>{preview.size / 1000} kB</i> <br />
              <br />
              <Button
                icon='select'
                disabled={calling}
                type='primary'
                onClick={async () => {
                  try {
                    setCalling(true);
                    await SnapshotService.applyPreviewAsync(preview.name);
                  } finally {
                    setCalling(false);
                  }
                }}
              >
                Apply
              </Button>
              <br />
              <br />
              <Button
                disabled={calling}
                type='dashed'
                icon='download'
                onClick={async () => {
                  try {
                    setCalling(true);
                    await SnapshotService.download(preview.name);
                    refresh();
                  } finally {
                    setCalling(false);
                  }
                }}
              >
                Download
              </Button>
              <br />
              <br />
              <Popconfirm
                title='Are you sure you want to delete this snapshot?'
                onConfirm={async () => {
                  try {
                    setCalling(true);
                    await SnapshotService.deleteSnapShotAsync(preview.name);
                    refresh();
                  } finally {
                    setCalling(false);
                  }
                }}
                okText='Yes'
                cancelText='No'
              >
                <Button disabled={calling} type='danger'>
                  Delete
                </Button>
              </Popconfirm>
            </Card>
          </List.Item>
        )}
      />
      ,
    </ContentBase>
  );
};

export default Home;
