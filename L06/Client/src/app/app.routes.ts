import { Routes } from "@angular/router";

import { HomeComponent } from "@/pages/home/home.component";
import { ThreadComponent } from "@/pages/thread/thread.component";
import { ReplyComponent } from "@/pages/reply/reply.component";
import { CreateThreadComponent } from "@/pages/thread/create/create-thread.component";
import { EditThreadComponent } from "@/pages/thread/edit/edit-thread.component";
import { DeleteThreadComponent } from "@/pages/thread/delete/delete-thread.component";
import { CreateReplyComponent } from "@/pages/reply/create/create-reply.component";
import { EditReplyComponent } from "@/pages/reply/edit/edit-reply.component";
import { DeleteReplyComponent } from "@/pages/reply/delete/delete-reply.component";

export const routes: Routes = [
  {
    path: "",
    component: HomeComponent,
  },
  {
    path: "thread",
    component: ThreadComponent,
  },
  {
    path: "thread/create",
    component: CreateThreadComponent,
  },
  {
    path: "thread/edit",
    component: EditThreadComponent,
  },
  {
    path: "thread/delete",
    component: DeleteThreadComponent,
  },
  {
    path: "reply",
    component: ReplyComponent,
  },
  {
    path: "reply/create",
    component: CreateReplyComponent,
  },
  {
    path: "reply/edit",
    component: EditReplyComponent,
  },
  {
    path: "reply/delete",
    component: DeleteReplyComponent,
  },
];
