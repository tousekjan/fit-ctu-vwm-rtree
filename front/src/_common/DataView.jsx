import React from 'react';
import { Collapse } from 'antd';

const { Panel } = Collapse;

const DataView = ({ data }) => {
  const writePoint = point => (
    <div>
      {point.coords.reduce(
        (previous, current) =>
          previous === '(' ? previous + current : previous + `, ${current}`,
        '('
      )}
      )
    </div>
  );

  const writeNodes = (node, level) => {
    return (
      <Panel
        key={node.id}
        header={
          <p>
            <b>Level:</b> {level}, <b>ID:</b> {node.id},{' '}
            <b>Bounding rectangle (min,max)s: </b>
            {node.boundingRectangle &&
              node.boundingRectangle.coords.reduce(
                (previous, current) =>
                  previous === '('
                    ? previous + `(${current.item1} ${current.item2})`
                    : previous + `, (${current.item1} ${current.item2})`,
                '('
              )}
            )
            {node.boundingRectangle && (
              <>
                <b> Area:</b> {node.boundingRectangle.area}
              </>
            )}
          </p>
        }
        style={{ marginLeft: '1em' }}
      >
        <Collapse>
          {node.childrenIds
            ? node.childrenIds.map(id => writeNodes(data.nodes[id], level + 1))
            : node.points.map(writePoint)}
        </Collapse>
      </Panel>
    );
  };

  if (data.shrinked)
    return (
      <p>
        Data is too large to be displayed, there is already '{data.nodeCount}'
        nodes!{' '}
      </p>
    );

  return <Collapse>{writeNodes(data.nodes[data.rootNodeId], 0)}</Collapse>;
};

export default DataView;
