insert into public.bank (swift_code, name, country_code)
values
    ('SWEDSESS', 'Swedbank', 'SE'),
    ('HANDSESS', 'Handelsbanken', 'SE'),
    ('ESSESESS', 'SEB', 'SE')
on conflict (swift_code) do update
    set
        name = excluded.name,
        country_code = excluded.country_code
where (bank.name, bank.country_code) is distinct from (excluded.name, excluded.country_code);
