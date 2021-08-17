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
            <Collapse>
              <Panel header={`Points ( ${result.linearResultCount}  )  `}>
                {result.linearResult && result.linearResult.length > 0 ? (
                  result.linearResult.map(writePoint)
                ) : result.linearResultShrinked ? (
                  'Result is too large to be displayed'
                ) : (
                  <Empty />
                )}
              </Panel>
            </Collapse>
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
        <Collapse>
          <Panel header={`Points ( ${result.treeResultCount}  )  `}>
            {result.treeResult && result.treeResult.length > 0 ? (
              result.treeResult.map(writePoint)
            ) : result.treeResultShrinked ? (
              'Result is too large to be displayed'
            ) : (
              <Empty />
            )}
          </Panel>
        </Collapse>
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
