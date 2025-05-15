export function relativeTime(dateTime: Date): string {
  const now = new Date();
  const timeSpan = now.getTime() - dateTime.getTime();
  const seconds = timeSpan / 1000;
  const minutes = seconds / 60;
  const hours = minutes / 60;
  const days = hours / 24;
  const weeks = days / 7;
  const months = days / 30;
  const years = days / 365;

  if (years > 1) {
    return `${Math.floor(years)} years ago`;
  }
  if (months > 2) {
    return `${Math.floor(months)} months ago`;
  }
  if (weeks > 2) {
    return `${Math.floor(weeks)} weeks ago`;
  }
  if (days > 2) {
    return `${Math.floor(days)} days ago`;
  }
  if (hours > 2) {
    return `${Math.floor(hours)} hours ago`;
  }
  if (minutes > 2) {
    return `${Math.floor(minutes)} minutes ago`;
  }
  if (seconds > 2) {
    return `${Math.floor(seconds)} seconds ago`;
  }
  return "Now";
}
