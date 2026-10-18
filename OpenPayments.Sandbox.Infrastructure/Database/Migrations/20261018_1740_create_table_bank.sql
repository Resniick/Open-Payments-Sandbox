create table if not exists bank (
    id uuid primary key default uuidv7(),
    swift_code text not null unique,
    name text not null,
    country_code char(2) not null
);
