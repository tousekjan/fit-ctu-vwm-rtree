import { notification } from 'antd';

const _BaseAddress = 'http://localhost:8080/api/v1/rtree';

const notifyServerError = text => {
  notification['error']({
    message: 'Server error',
    description: text
  });
};

export const getInfoAsync = async () => {
  try {
    const response = await fetch(_BaseAddress + '/info', {
      method: 'GET',
      headers: getHeaders()
    });

    if (response.ok) return await response.json();

    notifyServerError('Error while trying to fetch RTree info.');
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }
};

export const getDataAsync = async () => {
  try {
    const response = await fetch(_BaseAddress + '/data', {
      method: 'GET',
      headers: getHeaders()
    });

    if (!response.ok)
      notifyServerError('Error while trying to fetch RTree info.');

    const data = await response.json();

    if (data.shrinked)
      return { shrinked: data.shrinked, nodeCount: data.nodeCount };

    const dataObj = {};

    data.structure.forEach(e => {
      dataObj[e.id] = e;
    });
    return { rootNodeId: data.rootNodeId, nodes: dataObj };
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }
};

export const putFreshAsync = async body => {
  try {
    var response = await fetch(_BaseAddress + '/fresh', {
      method: 'PUT',
      body: JSON.stringify(body),
      headers: getHeaders()
    });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }

  if (!response.ok)
    notifyServerError(
      'Error on the server side while trying to create fresh tree!'
    );
  else
    notification['success']({
      message: 'Fresh tree',
      description: 'Fresh tree successfuly created.'
    });
};


export const postPointAsync = async coords => {
  let dataArray = [];
  for (let o in coords) {
    dataArray.push(coords[o]);
  }

  try {
    var response = await fetch(_BaseAddress + '/points', {
      method: 'POST',
      body: JSON.stringify({ coords: dataArray }),
      headers: getHeaders()
    });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }

  if (!response.ok)
    notifyServerError(
      'Error on the server side while trying to add point to RTree!'
    );
  else
    notification['success']({
      message: 'Point added',
      description: 'Point was successfuly added to RTree.'
    });
};

export const getKNNAsync = async values => {
  let query = '';
  for (let o in values) {
    if (o === 'Count') query += 'Count=' + values[o] + '&';
    else query += 'Coords=' + values[o] + '&';
  }

  try {
    var response = await fetch(_BaseAddress + '/query/knn?' + query, {
      method: 'GET',
      headers: getHeaders()
    });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }

  if (!response.ok)
    notifyServerError('Error on the server side while trying perform query!');
  else
  {
    notification['success']({
      message: 'Query succeded',
      description: 'Query executed successfuly.'
    });
    return await response.json();
  }
};

export const getNNAsync = async values => {
  let query = '';
  for (let o in values) {
     query += 'Coords=' + values[o] + '&';
  }

  try {
    var response = await fetch(_BaseAddress + '/query/nn?' + query, {
      method: 'GET',
      headers: getHeaders()
    });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }

  if (!response.ok)
    notifyServerError('Error on the server side while trying perform query!');
  else
  {
    notification['success']({
      message: 'Query succeded',
      description: 'Query executed successfuly.'
    });
    return await response.json();
  }
};

export const getRangeAsync = async values => {
  let query = '';
  for (let o in values) {
    if (o === 'Range') query += 'Range=' + values[o] + '&';
    else query += 'Coords=' + values[o] + '&';
  }

  try {
    var response = await fetch(_BaseAddress + '/query/range?' + query, {
      method: 'GET',
      headers: getHeaders()
    });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }

  if (!response.ok)
    notifyServerError('Error on the server side while trying perform query!');
  else
  {
    notification['success']({
      message: 'Query succeded',
      description: 'Query executed successfuly.'
    });
    return await response.json();
  }
};

export const containsAsync = async values => {
  let query = '';
  for (let o in values) {
   query += 'Coords=' + values[o] + '&';
  }

  try {
    var response = await fetch(_BaseAddress + '/query/contains?' + query, {
      method: 'GET',
      headers: getHeaders()
    });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }

  if (!response.ok)
    notifyServerError('Error on the server side while trying perform query!');
  else
  {
    notification['success']({
      message: 'Query succeded',
      description: 'Query executed successfuly.'
    });
    return await response.json();
  }
};

export const getBenchmarkRangeAsync = async values => {
  let query = '';
  for (let o in values) {
    if (o === 'Range') query += 'Range=' + values[o] + '&';
    else query += 'Coords=' + values[o] + '&';
  }

  try {
    var response = await fetch(_BaseAddress + '/query/range/benchmark?' + query, {
      method: 'GET',
      headers: getHeaders()
    });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }

  if (!response.ok)
    notifyServerError('Error on the server side while trying perform query!');
  else
  {
    notification['success']({
      message: 'Query succeded',
      description: 'Query executed successfuly.'
    });
    return await response.json();
  }
};

export const getBenchmarkKNNAsync = async values => {
  let query = '';
  for (let o in values) {
    if (o === 'Count') query += 'Count=' + values[o] + '&';
    else query += 'Coords=' + values[o] + '&';
  }

  try {
    var response = await fetch(_BaseAddress + '/query/knn/benchmark?' + query, {
      method: 'GET',
      headers: getHeaders()
    });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }

  if (!response.ok)
    notifyServerError('Error on the server side while trying perform query!');
  else
  {
    notification['success']({
      message: 'Query succeded',
      description: 'Query executed successfuly.'
    });
    return await response.json();
  }
};

export const getBenchmarkNNAsync = async values => {
  let query = '';
  for (let o in values) {
     query += 'Coords=' + values[o] + '&';
  }

  try {
    var response = await fetch(_BaseAddress + '/query/nn/benchmark?' + query, {
      method: 'GET',
      headers: getHeaders()
    });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }

  if (!response.ok)
    notifyServerError('Error on the server side while trying perform query!');
  else
  {
    notification['success']({
      message: 'Query succeded',
      description: 'Query executed successfuly.'
    });
    return await response.json();
  }
};

const getHeaders = () => {
  return new Headers([
    ['Accept', 'application/json'],
    ['Content-Type', 'application/json']
  ]);
};
