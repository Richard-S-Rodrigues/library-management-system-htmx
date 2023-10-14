CREATE TABLE IF NOT EXISTS "book"(
  "id" serial primary key,
  "isbn" text not null,
  "title" text not null,
  "author" text not null,
  "description" text not null,
  "created_at" timestamp not null,
  "updated_at" timestamp not null
);

CREATE TABLE IF NOT EXISTS "member"(
  "id" serial primary key,
  "name" text not null,
  "email" text not null,
  "address" text not null,
  "max_book_limit" int not null,
  "created_at" timestamp not null,
  "updated_at" timestamp not null
);

CREATE TABLE IF NOT EXISTS "issued_book"(
  "id" serial primary key,
  "member_id" serial,
  "book_id" serial,
  "issued_date" timestamp not null,
  "return_date" timestamp not null,
  FOREIGN KEY ("member_id") REFERENCES "member"("id"),
  FOREIGN KEY ("book_id") REFERENCES "book"("id") 
);