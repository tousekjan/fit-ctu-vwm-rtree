import React from 'react';
import { Empty } from 'antd';

import { Stage, Layer, Rect, Circle } from 'react-konva';

const Colors = ['green', 'orange', 'yellow'];

const DataVisualize = ({ data, info }) => {
  if (info.dimensions > 2)
    return (
      <>
        <p>Visualization is allowed only on tree with 2 dimensions</p>
      </>
    );

  if (data.shrinked)
    return (
      <>
        Data is too large to be displayed, there is already '{data.nodeCount}'
        nodes!{' '}
      </>
    );

  const rootNode = data.nodes[data.rootNodeId];

  if (!rootNode.boundingRectangle) return <Empty />;

  const toZeroX = rootNode.boundingRectangle.coords[0].item1;
  const toZeroY = rootNode.boundingRectangle.coords[1].item1;

  const toRatioX =
    (rootNode.boundingRectangle.coords[0].item2 -
      rootNode.boundingRectangle.coords[0].item1) /
    770;

  const toRatioY =
    (rootNode.boundingRectangle.coords[1].item2 -
      rootNode.boundingRectangle.coords[1].item1) /
    390;

  const writePoint = point => (
    <Circle
      x={(point.coords[0] - toZeroX) / toRatioX}
      y={(point.coords[1] - toZeroY) / toRatioY}
      radius={5}
      fill='blue'
    />
  );

  const writeNodes = (node, level) => {
    return (
      <>
        <Rect
          x={(node.boundingRectangle.coords[0].item1 - toZeroX) / toRatioX}
          y={(node.boundingRectangle.coords[1].item1 - toZeroY) / toRatioY}
          width={
            (node.boundingRectangle.coords[0].item2 -
              node.boundingRectangle.coords[0].item1) /
            toRatioX
          }
          height={
            (node.boundingRectangle.coords[1].item2 -
              node.boundingRectangle.coords[1].item1) /
            toRatioY
          }
          stroke={Colors[level % Colors.length]}
        />
        {node.childrenIds
          ? node.childrenIds.map(id => writeNodes(data.nodes[id], level + 1))
          : node.points.map(writePoint)}
      </>
    );
  };

  return (
    <>
      <Stage
        style={{ padding: '1em', backgroundColor: '#f0f2f5' }}
        width={800}
        height={400}
      >
        <Layer>
          <Rect
            x={
              (rootNode.boundingRectangle.coords[0].item1 - toZeroX) / toRatioX
            }
            y={
              (rootNode.boundingRectangle.coords[1].item1 - toZeroY) / toRatioY
            }
            width={
              (rootNode.boundingRectangle.coords[0].item2 -
                rootNode.boundingRectangle.coords[0].item1) /
              toRatioX
            }
            height={
              (rootNode.boundingRectangle.coords[1].item2 -
                rootNode.boundingRectangle.coords[1].item1) /
              toRatioY
            }
            stroke='black'
          />
          {rootNode.childrenIds ? (
            rootNode.childrenIds.map(id => writeNodes(data.nodes[id], 0))
          ) : (
            <div />
          )}
        </Layer>
      </Stage>
      <p>
        'Black' = root <br />
        'Green' = level 1<br />
        'Orange' = level 2<br />
        'Yellow' = level 3
      </p>
    </>
  );
};

export default DataVisualize;
