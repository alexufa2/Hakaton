export function fixDrawerHtmlScroll (flag = true)
{
  let html = document.getElementsByTagName('html')[0] as any;

  if (flag)
  {
    html.style.overflowY = 'hidden';
    html.style.paddingRight = html.scrollHeight > html.clientHeight ? '17px' : '0';
  }
  else
  {
    html.style.paddingRight = '0';
    html.style.overflowY = 'visible';
  }
}
