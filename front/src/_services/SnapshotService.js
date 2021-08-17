import { notification } from 'antd';

const _BaseAddress = 'http://localhost:8080/api/v1/rtree';

const notifyServerError = text => {
  notification['error']({
    message: 'Server error',
    description: text
  });
};

export const getPreviewsAsync = async () => {
  try {
    const response = await fetch(_BaseAddress + '/snapshots', {
      method: 'GET',
      headers: getHeaders()
    });

    if (response.ok) return await response.json();

    notifyServerError('Error while trying to fetch RTree snapshots.');
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }
};

export const applyPreviewAsync = async filename => {
  try {
    const response = await fetch(
      _BaseAddress + '/snapshots/' + filename.replace('.json', '') + '/apply',
      {
        method: 'PUT',
        headers: getHeaders()
      }
    );

    if (!response.ok)
      notifyServerError('Error while trying to apply snapshot.');
    else
      notification['success']({
        message: 'Snapshot applied',
        description:
          "Snapshot '" +
          filename.replace('.json', '') +
          "' applied successully."
      });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }
};

export const deleteSnapShotAsync = async filename => {
  try {
    const response = await fetch(
      _BaseAddress + '/snapshots/' + filename.replace('.json', ''),
      {
        method: 'DELETE',
        headers: getHeaders()
      }
    );

    if (!response.ok)
      notifyServerError('Error while trying to delete snapshot.');
    else
      notification['success']({
        message: 'Snapshot deleted',
        description:
          "Snapshot '" +
          filename.replace('.json', '') +
          "' deleted successully."
      });
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }
};

export const postSnapShotAsync = async filename => {
  try {
    const response = await fetch(
      _BaseAddress + '/snapshots/' + filename.replace('.json', ''),
      {
        method: 'POST',
        headers: getHeaders()
      }
    );

    if (response.status === 409) {
      notification['warning']({
        message: 'Snapshot already exists',
        description:
          "Snapshot with name '" +
          filename.replace('.json', '') +
          "' already exists."
      });
      return false;
    } else if (!response.ok)
      notifyServerError('Error while trying to create snapshot.');
    else
      notification['success']({
        message: 'Snapshot create',
        description:
          "Snapshot '" +
          filename.replace('.json', '') +
          "' created successully."
      });

    return true;
  } catch (err) {
    notifyServerError('Error while calling server api.');
    throw err;
  }
};

export const download = filename => {
  fetch(_BaseAddress + '/snapshots/' + filename.replace('.json', '')).then(
    response => {
      response.blob().then(blob => {
        let url = window.URL.createObjectURL(blob);
        let a = document.createElement('a');
        a.href = url;
        a.download = filename;
        a.click();
      });
    }
  );
};

const getHeaders = () => {
  return new Headers([
    ['Accept', 'application/json'],
    ['Content-Type', 'application/json']
  ]);
};
