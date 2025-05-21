type Thread = {
  author: string;
  createdAt: string;
  description: string;
  id: string;
  replies: any[];
  title: string;
  updatedAt: string;
};

type Reply = {
  author: string;
  createdAt: string;
  content: string;
  id: string;
  threadId: string;
  updatedAt: string;
};
