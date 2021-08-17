import React from 'react';

import { Collapse, Empty } from 'antd';

const { Panel } = Collapse;

const writePoint = point => (
  <div key={point.coords}>
    {point.coords.reduce(
      (previous, current) =>
        previous === '(' ? previous + current : previous + `, ${current}`,
      '('
    )}
    )
  </div>
);

const writePointString = point =>
  point.coords.reduce(
    (previous, current) =>
      previous === '(' ? previous + current : previous + `, ${current}`,
    '('
  ) + ')';

const QueryResult = ({ result }) => (
  <>
    <p>
      <i>
        Range: {result.requestRange}, Point{' '}
        {writePointString(result.requestPoint)}
      </i>
    </p>
    <Collapse>
      {result.linearMilliseconds !== null &&
        result.linearMilliseconds !== undefined && (
          <Panel header='Linear Query'>
            <p>
              Time: <i>{result.linearMilliseconds.toString()}</i> ms
            </p>
            <p>
              {result.linearResult ? (
                writePoint(result.linearResult)
              ) : (
                <Empty />
              )}
            </p>
          </Panel>
        )}
      <Panel header='Tree Query'>
        <p>
          Time:{' '}
          <i>
            {result.treeMilliseconds !== null &&
            result.treeMilliseconds !== undefined
              ? result.treeMilliseconds.toString()
              : 'N/A'}
          </i>{' '}
          ms
        </p>
        <p>
          {result.treeResult ? writePoint(result.treeResult) : <Empty />}
        </p>
      </Panel>
      {result.linearResult !== undefined && (
        <Panel header='Compare'>
          <p>
            <b>Results idenical:</b>{' '}
            {result.resultsIdentical ? 'true' : 'false'}
          </p>
        </Panel>
      )}
    </Collapse>
  </>
);

export default QueryResult;
