export function convertDateTime(timeStamp) {
  const d = new Date(
    parseInt(timeStamp.replace('/Date(', '').replace(')/', ''), 10)
  );
  let dd, mm, yy;
  if (d.getDate() < 10) {
    dd = '0' + d.getDate().toString();
  } else {
    dd = d.getDate().toString();
  }
  if (d.getMonth() + 1 < 10) {
    mm = '0' + (d.getMonth() + 1).toString();
  } else {
    mm = (d.getMonth() + 1).toString();
  }
  yy = d.getFullYear().toString();
  return mm + '/' + dd + '/' + yy;
}

export function convertDateTimewithTime(timeStamp) {
  const d = new Date(
    parseInt(timeStamp.replace('/Date(', '').replace(')/', ''), 10)
  );
  let dd, mm, yy;
  if (d.getDate() < 10) {
    dd = '0' + d.getDate().toString();
  } else {
    dd = d.getDate().toString();
  }
  if (d.getMonth() + 1 < 10) {
    mm = '0' + (d.getMonth() + 1).toString();
  } else {
    mm = (d.getMonth() + 1).toString();
  }
  yy = d.getFullYear().toString();
  return mm + '/' + dd + '/' + yy + ' ' + formatAMPM(d);
}

export function convertDateTimewithOnlyTime(timeStamp) {
  const d = new Date(parseInt(timeStamp, 10));
  let dd, mm, yy;
  if (d.getDate() < 10) {
    dd = '0' + d.getDate().toString();
  } else {
    dd = d.getDate().toString();
  }
  if (d.getMonth() + 1 < 10) {
    mm = '0' + (d.getMonth() + 1).toString();
  } else {
    mm = (d.getMonth() + 1).toString();
  }
  yy = d.getFullYear().toString();
  return mm + '/' + dd + '/' + yy + ' ' + formatAMPM(d);
}

export function formatAMPM(date) {
  let hours = date.getHours();
  let minutes = date.getMinutes();
  const ampm = hours >= 12 ? 'PM' : 'AM';
  hours = hours % 12;
  hours = hours ? hours : 12; // the hour '0' should be '12'
  minutes = minutes < 10 ? '0' + minutes : minutes;
  const strTime = hours + ':' + minutes + ' ' + ampm;
  return strTime;
}

export function formatDate(date) {
  let fMM, fDD, fYY;
  fMM =
    date.getMonth() + 1 < 10
      ? '0' + (date.getMonth() + 1)
      : date.getMonth() + 1;
  fDD = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
  fYY = date.getFullYear();
  return fMM + '/' + fDD + '/' + fYY;
}
