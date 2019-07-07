export interface ResponseInterface {
	data: any;
	message: string;
	success : boolean
}

export class ReclameCabinet
{
  id : string;
  login : string;
  amount : string;

	constructor (data)
	{
		Object.assign(this, data);
	}
}

export class SearchResult
{
  id : string;
  fullAddress : string;
  streetName : string;

  constructor (data)
  {
    Object.assign(this, data);
  }
}


